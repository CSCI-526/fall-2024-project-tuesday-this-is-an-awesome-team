using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject playerPrefab;

    private int latestCheckpointID = -1;
    private Vector3 latestCheckpointPosition;

    private void Awake()
    {
        Instance = this;
    }

    public void Restart()
    {
        if (latestCheckpointID != -1)
        {
            Respawn();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void UpdateLatestCheckpoint(int checkpointID, Vector3 position)
    {
        if (checkpointID > latestCheckpointID)
        {
            latestCheckpointID = checkpointID;
            latestCheckpointPosition = position;
            Debug.Log("Latest Checkpoint updated to ID: " + latestCheckpointID);
        }
    }

    public IEnumerator Die()
    {
        UIManager.Instance.loseText.SetActive(true);
        yield return new WaitForSeconds(2f);
        Restart();
    }

    private void Respawn()
    {
        GameObject newPlayer = Instantiate(playerPrefab, latestCheckpointPosition, Quaternion.identity);
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = newPlayer.transform;
        UIManager.Instance.loseText.SetActive(false);
        Text cooldownText = GameObject.Find("Canvas").GetComponentInChildren<Text>();
        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(false);
        }
        Debug.Log("Player respawned at Checkpoint " + latestCheckpointID);
    }
}
