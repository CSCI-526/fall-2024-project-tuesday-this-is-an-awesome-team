using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool canExplode = true;
    public bool destroysWalls = false;
    public bool canHurtInstigator = false;
    public GameObject instigator = null;
    private bool isEnemyDead = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == instigator && !canHurtInstigator)
        {
            return;
        }

        if (collision.collider.CompareTag("Player"))
        {
            Health hpOfOther = collision.collider.GetComponent<Health>(); // the health point of the incoming collider

            if (isEnemyDead) 
            {
                return;
            }

            if (hpOfOther)
            {
                Debug.Log("Damaging: " + collision.gameObject.name);
                hpOfOther.Damage(damage, gameObject);
            }

            if (canExplode)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.collider.CompareTag("Enemy"))
        {
            Health hpOfOther = collision.collider.GetComponent<Health>();
            if (hpOfOther)
            {
                hpOfOther.Damage(damage, gameObject);
            }

            if (canExplode)
            {
                Destroy(gameObject);
            }
        }
        else if (destroysWalls)
        {
            if (collision.collider.CompareTag("Destructible"))
            {
                if (PlayerController.Instance.StuckSurface == collision.gameObject)
                {
                    PlayerController.Instance.Jump(Vector3.zero);
                }

                collision.gameObject.SetActive(false);
                SaveObject save = collision.collider.GetComponent<SaveObject>();
                if (save)
                {
                    save.State = true;
                }
                Destroy(gameObject);
            }
            if (!collision.collider.CompareTag("Gum"))
            {
                Destroy(gameObject);
            }
        }
        else if (collision.collider.CompareTag("Surface"))
        {
            if (GetComponent<Projectile>() != null)
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator Launch(GameObject instigator, float cooldown)
    {
        Debug.Log("Damage cooldown");
        this.instigator = instigator;
        canHurtInstigator = false;
        yield return new WaitForSeconds(cooldown);
        if (this.instigator == instigator)
        {
            canHurtInstigator = true;
        }
    }

    void Update()
    {
        // if the current object is an enemy
        if (CompareTag("Enemy"))
        {
            Health hpOfThis = GetComponent<Health>(); // the health point of the current enemy
            if (hpOfThis.health <= 0) // if the enemy already died
            {
                isEnemyDead = true;
                gameObject.layer = LayerMask.NameToLayer("DeadEnemy"); // the DeadEnemy layer doesn't have collision with Player layer
            }
        }
    }
}
