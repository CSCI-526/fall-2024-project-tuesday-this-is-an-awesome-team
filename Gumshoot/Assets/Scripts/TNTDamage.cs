using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool canExplode = true;
    public bool canHurtInstigator = false;
    public GameObject instigator = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == instigator && !canHurtInstigator)
        {
            return;
        }

        if (collision.collider.CompareTag("Enemy"))
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
