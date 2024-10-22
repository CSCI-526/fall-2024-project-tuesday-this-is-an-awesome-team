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

    [HideInInspector] public int latestCheckpointID = -1;
    private Vector3 latestCheckpointPosition;

    [HideInInspector] public static int[] deathPerCheckpoint = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    [HideInInspector] public static List<Vector3> deathLocationListLevel0 = new List<Vector3>();
    [HideInInspector] public static List<Vector3> deathLocationListLevel1 = new List<Vector3>();
    [HideInInspector] public static List<Vector3> deathLocationListLevelMain = new List<Vector3>();

    [SerializeField] private string NextLevel = "";

    private void Awake()
    {
        Instance = this;
    }

    public void LoadNextLevel()
    {
        if (NextLevel.Length > 0)
        {
            SceneManager.LoadScene(NextLevel);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void UpdateDeathPerCheckpoint()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);

        if (sceneName == "Level 0")
        {
            deathPerCheckpoint[0] += 1;
        }
        else if (sceneName == "Level 1")
        {
            deathPerCheckpoint[1] += 1;
        }
        else if (sceneName == "Master")
        {
            deathPerCheckpoint[latestCheckpointID + 3] += 1;
        }
    }

    public void AddDeathLocation(Vector3 deathPosition)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Level 0")
        {
            deathLocationListLevel0.Add(deathPosition);
            Debug.Log("Death location added to Level 0: " + deathPosition);
        }
        else if (sceneName == "Level 1")
        {
            deathLocationListLevel1.Add(deathPosition);
            Debug.Log("Death location added to Level 1: " + deathPosition);
        }
        else if (sceneName == "Master")
        {
            deathLocationListLevelMain.Add(deathPosition);
            Debug.Log("Death location added to Master: " + deathPosition);
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
        UpdateDeathPerCheckpoint();
        yield return new WaitForSeconds(2f);
        Respawn();


    }

    public void Respawn()
    {
        if (latestCheckpointID != -1)
        {
            GameObject newPlayer = Instantiate(playerPrefab, latestCheckpointPosition, Quaternion.identity);
            GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = newPlayer.transform;
            UIManager.Instance.loseText.SetActive(false);
            UIManager.Instance.countdownText.text = "";
            Debug.Log("Player respawned at Checkpoint " + latestCheckpointID);
        }
        else
        {
            Restart();
        }
    }
}
