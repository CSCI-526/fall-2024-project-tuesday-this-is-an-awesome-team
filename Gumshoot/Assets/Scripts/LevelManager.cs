using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject playerPrefab;

    [HideInInspector] public int latestCheckpointID = -1;
    private Vector3 latestCheckpointPosition;

    [HideInInspector] public static Dictionary<string, LevelData> levelsData = new Dictionary<string, LevelData>();
    [SerializeField] private string NextLevel = "";
    private float currentStartTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartTimer(); 
    }

    public void StartTimer()
    {
        currentStartTime = Time.time; 
    }

    public void EndTimer(int checkpointID)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!levelsData.ContainsKey(sceneName))
            levelsData[sceneName] = new LevelData();
        if (currentStartTime > 0)
        {
            float timeSpent = Time.time - currentStartTime;
            levelsData[sceneName].RecordCheckpointTime(checkpointID, timeSpent);
            currentStartTime = 0;
        }
    }


    public void TriggerEndDoor()
    {
        EndTimer(latestCheckpointID + 1);
        TriggerCheckpoint(latestCheckpointID + 1);
        Debug.Log("End door reached, level completed.");
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
        ResetData();
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
        if (!levelsData.ContainsKey(sceneName))
            levelsData[sceneName] = new LevelData();

        levelsData[sceneName].UpdateEnemyControllerUsage(latestCheckpointID, EnemyType);
    }

    public static void ResetData()
    {
        foreach (var levelData in levelsData.Values)
        {
            levelData.ResetData();
        }
    }

    public void AddDeathLocation(Vector2 deathPosition)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!levelsData.ContainsKey(sceneName))
            levelsData[sceneName] = new LevelData();

        levelsData[sceneName].AddDeathLocation(deathPosition);
        Debug.Log($"Death location added to {sceneName}: {deathPosition}");
    }

    public void UpdateLatestCheckpoint(int checkpointID, Vector3 position)
    {
        if (checkpointID > latestCheckpointID)
        {
            latestCheckpointID = checkpointID;
            latestCheckpointPosition = position;
            TriggerCheckpoint(checkpointID);
            EndTimer(latestCheckpointID);
            latestCheckpointID = checkpointID;
            StartTimer();
            Debug.Log("Latest Checkpoint updated to ID: " + latestCheckpointID);
        }
    }

    public void RecordCheckpointTime(int checkpointID, float timeSpent)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!levelsData.ContainsKey(sceneName))
        {
            levelsData[sceneName] = new LevelData();
        }

        levelsData[sceneName].RecordCheckpointTime(checkpointID, timeSpent);
        Debug.Log($"Time recorded for checkpoint {checkpointID} in {sceneName}: {timeSpent} seconds");
    }

    public void TriggerCheckpoint(int checkpointID)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!levelsData.ContainsKey(sceneName))
        {
            levelsData[sceneName] = new LevelData();
        }

        levelsData[sceneName].TriggerCheckpoint(checkpointID);
        Debug.Log($"Checkpoint {checkpointID} triggered in {sceneName}");
    }


    public IEnumerator Die()
    {
        UIManager.Instance.loseText.SetActive(true);
        yield return new WaitForSeconds(1f);
        Respawn();
    }

    public void Respawn()
    {
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



[System.Serializable]
public class LevelData
{
    public List<Vector2> deathLocations = new List<Vector2>();
    public Dictionary<int, CheckpointData> checkpointData = new Dictionary<int, CheckpointData>();

    public void AddDeathLocation(Vector2 deathPosition)
    {
        if (deathLocations == null)
        {
            deathLocations = new List<Vector2>();
        }
        deathLocations.Add(deathPosition);
    }

    public void UpdateEnemyControllerUsage(int checkpointID, int enemyType)
    {
        if (!checkpointData.ContainsKey(checkpointID))
        {
            checkpointData[checkpointID] = new CheckpointData();
        }
        checkpointData[checkpointID].IncreaseEnemyControllerUsage(enemyType);
    }

    public void RecordCheckpointTime(int checkpointID, float timeSpent)
    {
        if (!checkpointData.ContainsKey(checkpointID))
        {
            checkpointData[checkpointID] = new CheckpointData();
        }
        checkpointData[checkpointID].AddTimeSpent(timeSpent);
    }

    public void TriggerCheckpoint(int checkpointID)
    {
        if (!checkpointData.ContainsKey(checkpointID))
        {
            checkpointData[checkpointID] = new CheckpointData();
        }

        checkpointData[checkpointID].SetTriggered();
    }

    public void ResetData()
    {
        deathLocations?.Clear();
        foreach (var checkpoint in checkpointData.Values)
        {
            checkpoint.ResetData();
        }
    }
}

[System.Serializable]
public class CheckpointData
{
    private Dictionary<int, int> enemyControllerUsage = new Dictionary<int, int>();
    public List<float> timeSpentList = new List<float>();
    public bool isTriggered = false;

    public void AddTimeSpent(float time)
    {
        timeSpentList.Add(time);
    }

    public void SetTriggered()
    {
        isTriggered = true;
    }
    
    public bool HasTriggerData()
    {
        return isTriggered;
    }

    public bool HasTimeSpentData()
    {
        return timeSpentList.Count > 0;
    }

    public bool HasEnemyUsageData()
    {
        return enemyControllerUsage.Count > 0;
    }

    public void IncreaseEnemyControllerUsage(int enemyType)
    {
        if (!enemyControllerUsage.ContainsKey(enemyType))
        {
            enemyControllerUsage[enemyType] = 0;
        }
        enemyControllerUsage[enemyType]++;
    }

    public void ResetData()
    {
        enemyControllerUsage.Clear();
        timeSpentList.Clear();
        isTriggered = false;
    }

    public List<string> GetEnemyControllerUsage()
    {
        List<string> usageList = new List<string>();

        foreach (var usage in enemyControllerUsage)
        {
            usageList.Add($"Type {usage.Key}: {usage.Value} times");
        }

        return usageList;
    }
}