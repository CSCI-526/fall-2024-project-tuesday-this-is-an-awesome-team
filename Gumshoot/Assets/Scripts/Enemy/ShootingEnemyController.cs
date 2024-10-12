using UnityEngine;
/*This class contorls shooting enemy. It uses drection based movement method and has the ability to shoot. It can retate itself to shoot in different directions and shoot a projectile by pressing "enter/return" key*/
public class ShootingEnemyController : EnemyControllerBase
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 50f; 
    private ShootingControl shootingControl;

    protected override void Start()
    {
        base.Start();
        movementStrategy = new DirectionMovementController();
        shootingControl = new ShootingControl(projectilePrefab, firePoint, fireRate);
    }

    protected override void Update()
    {
        base.Update();
        shootingControl.UpdateCooldown();
        if (Input.GetKeyDown(KeyCode.Space) && shootingControl.CanShoot())
            HandleShooting();
    }

    private void HandleShooting()
    {
        Vector2 shootDirection = transform.up; 
        shootingControl.Shoot(shootDirection);
    }
}
