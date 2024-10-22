using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.UpdateLatestCheckpoint(checkpointID, transform.position);
            Debug.Log("Reached checkpoint " + checkpointID);
            Debug.Log("Current position " + transform.position);
            GetComponent<SpriteRenderer>().color = Color.green;
            DataPersistanceManager.Instance.Save(transform.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
