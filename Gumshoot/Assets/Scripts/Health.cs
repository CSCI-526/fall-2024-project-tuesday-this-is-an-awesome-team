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
                Debug.Log(LevelManager.deathPerCheckpoint[LevelManager.Instance.latestCheckpointID + 1]);
                LevelManager.Instance.StartCoroutine(LevelManager.Instance.Die());
                Debug.Log(LevelManager.deathPerCheckpoint[LevelManager.Instance.latestCheckpointID + 1]);
            }
            Destroy(gameObject);
        }
    }

    public void Heal(int heal)
    {
        health += heal;
    }
}
