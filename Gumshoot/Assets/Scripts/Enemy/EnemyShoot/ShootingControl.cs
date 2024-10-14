using UnityEngine;
/*This class provides a basic shooting control. It deals with instantiating the projectile at the fire point, setting the projectile's moving direction, applying the cooldown mechanism to restrict shooting.*/
public class ShootingControl
{
    private GameObject projectilePrefab;
    private Transform firePoint;
    private float fireRate; 
    private float fireCooldown;
    private bool coolDown;
    
    
    public ShootingControl(GameObject projectilePrefab, Transform firePoint, float fireRate, bool coolDown)
    {
        this.projectilePrefab = projectilePrefab;
        this.firePoint = firePoint;
        this.fireRate = fireRate;
        this.fireCooldown = 0f; 
        this.coolDown = coolDown;
    }

    public void UpdateCooldown()
    {
        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    public bool CanShoot()
    {
        return fireCooldown <= 0f;
    }

    public void Shoot(Vector2 shootDirection)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            if (coolDown && CanShoot() || ! coolDown) {
                GameObject m_projectile = Object.Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Projectile projectile = m_projectile.GetComponent<Projectile>();
                if (projectile != null)
                    projectile.SetDirection(shootDirection);
            }
            if (coolDown) 
                fireCooldown = 1f / fireRate;
        }
    }
}
