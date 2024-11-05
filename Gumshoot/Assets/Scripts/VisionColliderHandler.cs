using UnityEngine;

public class VisionColliderHandler : MonoBehaviour
{
    private CircleCollider2D visionCollider;
    //public FlyingEnemyMovement flyingEnemy;
    private IEnemy enemy;

    private LineRenderer lineRenderer;

    [Header("Layer Settings")]
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;

    [Header("Line Renderer Settings")]
    public GameObject halo;

    public bool playerIsVisible = false;

    private Transform playerTransform;

    void Start()
    {
        visionCollider = GetComponent<CircleCollider2D>();
        //flyingEnemy = GetComponentInParent<FlyingEnemyMovement>();
        enemy = GetComponentInParent<IEnemy>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        halo.transform.localScale = visionCollider.radius * 2 * Vector3.one;
    }

    public bool HasOverlap()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (enemy == null || playerTransform == null)
            return false;

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        return hit.collider == null && Physics2D.OverlapCircle(transform.position, visionCollider.radius, playerLayer) != null;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && HasOverlap())
        {
            //flyingEnemy.StartChasing();
            playerIsVisible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsVisible = false;
            //flyingEnemy.StopChasing();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (visionCollider != null)
    //    {
    //        Gizmos.color = Color.yellow;
    //        float scaleFactor = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
    //        Gizmos.DrawWireSphere(transform.position, visionCollider.radius * scaleFactor);
    //    }

    //}
}
