using System.Collections;
using UnityEngine;
using System.Collections.Generic;
/*This class provides a basic shooting control. It deals with instantiating the projectile at the fire point, setting the projectile's moving direction, applying the cooldown mechanism to restrict shooting.*/
public class ShootingControl : MonoBehaviour
{
    private GameObject projectilePrefab;
    private Transform firePoint;
    private float fireCooldown;
    private bool hasCoolDown;
    private bool canShoot = true;
    
    
    public void init(GameObject projectilePrefab, Transform firePoint, float fireCooldown, bool coolDown)
    {
        this.projectilePrefab = projectilePrefab;
        this.firePoint = firePoint;
        this.fireCooldown = fireCooldown;
        this.hasCoolDown = coolDown;
    }

    public bool CanShoot()
    {
        return canShoot;
    }

    public void Shoot(Vector2 shootDirection, GameObject instigator)
    {
        Debug.Log("Shooting");
        if (projectilePrefab != null && firePoint != null)
        {
            if (CanShoot()) {
                GameObject m_projectile = Object.Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                if (instigator.name.Contains("ShootingEnemyTutorial"))
                {
                    m_projectile.name = "Projectile2"; // Special projectile name for "ShootingEnemyTutorial"
                }
                Projectile projectile = m_projectile.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.SetDirection(shootDirection);
                }
                DamageObject damager = m_projectile.GetComponent<DamageObject>();
                if (damager != null)
                {
                    StartCoroutine(damager.Launch(instigator, 1.0f));
                }

                if (hasCoolDown)
                {
                    Debug.Log("Starting cooldown");
                    StartCoroutine(StartCooldown());
                }
            }
        }
    }

    IEnumerator StartCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireCooldown);
        canShoot = true;
    }
}
