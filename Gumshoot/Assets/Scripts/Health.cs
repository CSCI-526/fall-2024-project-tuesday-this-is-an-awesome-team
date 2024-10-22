using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public bool destroyOnDeath = false;

    public void Damage(int damage, GameObject instigator)
    {
        if (instigator.GetComponent<PlayerController>())
        {
            if (instigator == instigator.GetComponent<PlayerController>().PulledObject) { return; }
        }
        health -= damage;
        
        if (health <= 0 && destroyOnDeath)
        {
            if (CompareTag("Player"))
            {
                Debug.Log("Killed by " + instigator.name);
                LevelManager.Instance.AddDeathLocation(new Vector2(transform.position.x, transform.position.y));
                LevelManager.Instance.StartCoroutine(LevelManager.Instance.Die());
            }
            Destroy(gameObject);
        }
    }

    public void Heal(int heal)
    {
        health += heal;
    }
}
