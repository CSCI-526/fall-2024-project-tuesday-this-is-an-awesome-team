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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == instigator && !canHurtInstigator)
        {
            return;
        }

        if (collision.collider.CompareTag("Player"))
        {
            Health hpObj = collision.collider.GetComponent<Health>();
            if (hpObj)
            {
                Debug.Log("Damaging: " + collision.gameObject.name);
                hpObj.Damage(damage, gameObject);
            }

            if (canExplode)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.collider.CompareTag("Enemy"))
        {
            Health hpObj = collision.collider.GetComponent<Health>();
            if (hpObj)
            {
                hpObj.Damage(damage, gameObject);
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
                collision.gameObject.SetActive(false);
                collision.gameObject.GetComponent<SaveObject>().State = true;
                Destroy(gameObject);
            }
            if (!collision.collider.CompareTag("Gum"))
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
}
