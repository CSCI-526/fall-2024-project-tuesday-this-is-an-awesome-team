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

    public static Transform player;
    public GameObject controllerPrefab;

    public GameObject ControllerPrefab => controllerPrefab;

    private Health health;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private bool canJump = true;
    public float jumpCooldown = 2f;
    private float cooldownTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        visionHandler = transform.Find("VisionCollider").GetComponent<VisionColliderHandler>();
    }

    void Update()
    {
        if (health.health <= 0)
        {
            rb.velocity = Vector2.zero;
            sprite.color = Color.yellow;
            return;
        }

        if (visionHandler.playerIsVisible && rb.velocity.y == 0)
        {
            JumpTowardsPlayer();
        }
    }

    void JumpTowardsPlayer()
    {
        if (player == null) return;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float horizontalDirection = Mathf.Sign(directionToPlayer.x);
        Vector2 jumpForceVector = new Vector2(horizontalDirection * horizontalForce, verticalForce);
        rb.AddForce(jumpForceVector, ForceMode2D.Impulse);

        canJump = false;
    }
}
