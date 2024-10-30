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

    //[HideInInspector] public static int[] deathPerCheckpoint = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    [HideInInspector] public static List<Vector2> deathLocationListLevel0 = new List<Vector2>();
    [HideInInspector] public static List<Vector2> deathLocationListLevel1 = new List<Vector2>();
    [HideInInspector] public static List<Vector2> deathLocationListLevel2 = new List<Vector2>();
    [HideInInspector] public static List<Vector2> deathLocationListLevel3 = new List<Vector2>();
    [HideInInspector] public static List<Vector2> deathLocationListLevelMain = new List<Vector2>();
    [HideInInspector] public static int[] EnemyControllerUse = new int[13 * 3];

    [SerializeField] private string NextLevel = "";

    private void Awake()
    {
        Instance = this;
    }

    public void LoadNextLevel()
    {
        if (NextLevel.Length > 0)
        {
            if (DataPersistanceManager.Instance)
            {
                Destroy(DataPersistanceManager.Instance.gameObject);
                DataPersistanceManager.Instance = null;
            }
            SceneManager.LoadScene(NextLevel);
        }
    }

    public void LoadMainMenu()
    {
        SendToGoogle.Instance.Send();
        ResetDeathLocationsAndUsage();
        if (DataPersistanceManager.Instance)
        {
            Destroy(DataPersistanceManager.Instance.gameObject);
            DataPersistanceManager.Instance = null;
        }
        
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateEnemyControllerUse(int EnemyType)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);

        if (sceneName == "Level 1")
        {
            EnemyControllerUse[EnemyType] += 1;
        }
        else if (sceneName == "Level 2")
        {
            EnemyControllerUse[EnemyType + 3 * (2 + latestCheckpointID)] += 1;
        }
        else if (sceneName == "Level 3")
        {
            EnemyControllerUse[EnemyType + 3 * 4 ] += 1;
        }
        else if (sceneName == "Master")
        {
            EnemyControllerUse[EnemyType + 3 * (6 + latestCheckpointID)] += 1;
        }
    }

    public static void ResetDeathLocationsAndUsage()
    {
        deathLocationListLevel0.Clear();
        deathLocationListLevel1.Clear();
        deathLocationListLevel2.Clear();
        deathLocationListLevel3.Clear();
        deathLocationListLevelMain.Clear();

        for (int i = 0; i < EnemyControllerUse.Length; i++)
        {
            EnemyControllerUse[i] = 0;
        }
    }

    /*private void UpdateDeathPerCheckpoint()
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
        else if (sceneName == "Level 2")
        {
            deathPerCheckpoint[2] += 1;
        }
        else if (sceneName == "Master")
        {
            deathPerCheckpoint[latestCheckpointID + 4] += 1;
        }
    }*/

    public void AddDeathLocation(Vector2 deathPosition)
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
        else if (sceneName == "Level 2")
        {
            deathLocationListLevel2.Add(deathPosition);
            Debug.Log("Death location added to Level 2: " + deathPosition);
        }
        else if (sceneName == "Level 3")
        {
            deathLocationListLevel3.Add(deathPosition);
            Debug.Log("Death location added to Level 3: " + deathPosition);
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
        //UpdateDeathPerCheckpoint();
        yield return new WaitForSeconds(1f);
        Respawn();
    }

    public void Respawn()
    {
        //UpdateDeathPerCheckpoint();
        //yield return new WaitForSeconds(2f);
        if (latestCheckpointID == -1)
        {
            if (DataPersistanceManager.Instance)
            {
                Destroy(DataPersistanceManager.Instance.gameObject);
                DataPersistanceManager.Instance = null;
            }

            Restart();
        }
        else
        {
            DataPersistanceManager.Instance.hasNewPos = true;
            Restart();
        }
    }
}
