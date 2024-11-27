using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
/*This class deals with the automatic shooting ability of the shooting enemy. 
 * It will only shoot when player is visible, and will make the fire point 0.9 away from itself facing the player and shoot towards the player. */
public class ShootingEnemyShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float firePointDistance = 0.9f;
    private ShootingControl shootingControl;
    private VisionColliderHandler visionColliderHandler;
    public Transform muzzle;

    void Start()
    {
        shootingControl = gameObject.AddComponent<ShootingControl>();
        shootingControl.init(projectilePrefab, firePoint, fireRate, true);
        visionColliderHandler = transform.Find("VisionCollider").GetComponent<VisionColliderHandler>();
    }

    void Update()
    {
        if (GetComponent<Health>().health <= 0 || PlayerController.Instance == null) { return; }

        Vector3 direction = (PlayerController.Instance.gameObject.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        muzzle.position = transform.position + direction * 0.5f;
        muzzle.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (visionColliderHandler != null && visionColliderHandler.playerIsVisible)
            TryShooting();
    }

    void TryShooting()
    {
        if (shootingControl.CanShoot())
        {
            AdjustFirePoint();
            Vector2 shootDirection = (PlayerController.Instance.transform.position - firePoint.position).normalized; 
            shootingControl.Shoot(shootDirection, gameObject); 
        }
    }

    void AdjustFirePoint()
    {
        if (PlayerController.Instance != null && firePoint != null)
        {
            Vector2 directionToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
            firePoint.position = transform.position + (Vector3)(directionToPlayer * firePointDistance);

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
