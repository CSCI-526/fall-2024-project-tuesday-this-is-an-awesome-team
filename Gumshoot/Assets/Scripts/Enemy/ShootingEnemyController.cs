using UnityEngine;
/*This class contorls shooting enemy. It uses flying movement method and has the ability to shoot. Fire point and muzzle can rotate around the shooting enemy based on the mouse position to shoot in different directions and shoot a projectile by pressing the "space" key*/
public class ShootingEnemyController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform muzzle;
    public float fireRate = 50f; 
    private ShootingControl shootingControl;

    private void Start()
    {
        shootingControl = new ShootingControl(projectilePrefab, firePoint, fireRate, false);
    }

    private void Update()
    {
        RotateTowardsMouse();
        if (Input.GetKeyDown(KeyCode.Space) && shootingControl.CanShoot())
            HandleShooting();
    }

    private void HandleShooting()
    {
        Vector2 shootDirection = firePoint.right; 
        shootingControl.Shoot(shootDirection);
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 directionToMouse = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        muzzle.position = transform.position + directionToMouse * 0.5f;
        firePoint.position = transform.position + directionToMouse * 1f;
        muzzle.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
