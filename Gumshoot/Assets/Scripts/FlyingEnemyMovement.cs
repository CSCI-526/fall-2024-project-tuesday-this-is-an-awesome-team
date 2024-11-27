using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    GameObject ControllerPrefab { get; }
    float ControlTime { get; }
    void OnDeath();
}

public class FlyingEnemyMovement : MonoBehaviour, IEnemy
{
    [Header("Movement Settings")]
    public float patrolSpeed = 1f;
    public float chaseSpeed = 2f;
    public float patrolRange = 5f;

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

    private Vector2 patrolStartPos;
    private bool isFacingRight = true;
    private int patrolDirection = 1; // 1 for right, -1 for left
    private float patrolLeftLimit;
    private float patrolRightLimit;

    private bool isChasing = false;
    private Vector2 currentVelocity = Vector2.zero; // Used for SmoothDamp velocity

    private Health health;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private bool isDead = false;

    void Start()
    {
        patrolStartPos = transform.position;
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        patrolLeftLimit = patrolStartPos.x - patrolRange / 2f;
        patrolRightLimit = patrolStartPos.x + patrolRange / 2f;

        visionHandler = transform.Find("VisionCollider").GetComponent<VisionColliderHandler>();
    }

    void Update()
    {
        if (isDead) { return; }
        isChasing = visionHandler.playerIsVisible;
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    /*public void StartChasing()
    {
        isChasing = true;
    }

    public void StopChasing()
    {
        isChasing = false;
    }*/

    void Patrol()
    {
        Vector2 movement = Vector2.right * patrolDirection * patrolSpeed * Time.deltaTime;
        transform.Translate(movement);
        
        Vector2 rayDirection = patrolDirection == 1 ? Vector2.right : Vector2.left;
        // Visualize Raycast
        //Debug.DrawRay(transform.position, rayDirection * 1f, Color.blue);
        // Detect if there's an obstacle in front
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 1f, obstacleLayer);

        // Hit an obstacle, reverse direction
        if (hit.collider != null)
        {
            FlipPatrolDirection();
        }

        if (patrolDirection == 1 && transform.position.x >= patrolRightLimit)
        {
            FlipPatrolDirection();
        }
        else if (patrolDirection == -1 && transform.position.x <= patrolLeftLimit)
        {
            FlipPatrolDirection();
        }
    }

    void ChasePlayer()
    {
        if (PlayerController.Instance == null)
            return;

        Vector2 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        Vector2 targetPosition = PlayerController.Instance.transform.position;

        // Smoothly chase using SmoothDamp, limiting to maximum chase speed
        transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 0.1f, chaseSpeed);

        if (direction.x > 0 && !isFacingRight)
            Flip();
        else if (direction.x < 0 && isFacingRight)
            Flip();
    }

    void FlipPatrolDirection()
    {
        patrolDirection *= -1;
        Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void OnDeath()
    {
        isDead = true;
        transform.Find("Face").gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        sprite.color = Color.white;
        sprite.sprite = deadSprite;
    }
}
