using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemyMovement : MonoBehaviour, IEnemy
{
    [Header("Movement Settings")]
    public float verticalForce = 5f;
    public float horizontalForce = 5f;

    [Header("Layer Settings")]
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;

    [Header("References")]
    public VisionColliderHandler visionHandler;

    [SerializeField] private GameObject controllerPrefab;
    [SerializeField] private float controlTime;
    [SerializeField] private Sprite deadSprite;

    public GameObject ControllerPrefab => controllerPrefab;
    public float ControlTime => controlTime;

    private Health health;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private bool isDead = false;

    //public float jumpCooldown = 2f;
    //private float cooldownTimer = 0f;

    void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        visionHandler = transform.Find("VisionCollider").GetComponent<VisionColliderHandler>();
    }

    void Update()
    {
        if (isDead) { return; }
        if (visionHandler.playerIsVisible && rb.velocity.y == 0)
        {
            JumpTowardsPlayer();
        }
    }

    public void OnDeath()
    {
        isDead = true;
        transform.Find("Face").gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        sprite.color = Color.white;
        sprite.sprite = deadSprite;
    }

    void JumpTowardsPlayer()
    {
        if (PlayerController.Instance == null) return;

        Vector2 directionToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
        float horizontalDirection = Mathf.Sign(directionToPlayer.x);
        Vector2 jumpForceVector = new Vector2(horizontalDirection * horizontalForce, verticalForce);
        rb.AddForce(jumpForceVector, ForceMode2D.Impulse);
    }
}
