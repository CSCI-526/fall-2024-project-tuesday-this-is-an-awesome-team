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

    // 用于存储每个level的数据
    [HideInInspector] public static Dictionary<string, LevelData> levelsData = new Dictionary<string, LevelData>();

    //[HideInInspector] public static int[] deathPerCheckpoint = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    /*[HideInInspector] public static List<Vector2> deathLocationListLevel0 = new List<Vector2>();
    [HideInInspector] public static List<Vector2> deathLocationListLevel1 = new List<Vector2>();
    [HideInInspector] public static List<Vector2> deathLocationListLevel2 = new List<Vector2>();
    [HideInInspector] public static List<Vector2> deathLocationListLevel3 = new List<Vector2>();
    [HideInInspector] public static List<Vector2> deathLocationListLevelMain = new List<Vector2>();
    [HideInInspector] public static int[] EnemyControllerUse = new int[13 * 3];*/

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
        if (levelsData == null || levelsData.Count == 0)
        {
            Debug.LogError("levelsData is empty or null!");
        }
        SendToGoogle.Instance.Send();
        //ResetDeathLocationsAndUsage();
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

    /*public void UpdateEnemyControllerUse(int EnemyType)
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
    }*/

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
        if (!levelsData.ContainsKey(sceneName))
            levelsData[sceneName] = new LevelData();

        levelsData[sceneName].AddDeathLocation(deathPosition);
        Debug.Log($"Death location added to {sceneName}: {deathPosition}");
        // 验证添加后的结果
        var locations = string.Join(", ", levelsData[sceneName].deathLocations.ConvertAll(v => $"{v.x}, {v.y}"));
        Debug.Log($"Updated death locations for {sceneName}: {locations}");
        if (levelsData == null || levelsData.Count == 0)
        {
            Debug.LogError("levelsDataaaaaaaaaaaa is empty or null!");
        }
    }


    /*public void AddDeathLocation(Vector2 deathPosition)
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
    }*/

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



[System.Serializable]
public class LevelData
{
    public List<Vector2> deathLocations = new List<Vector2>();
    public Dictionary<int, CheckpointData> checkpointData = new Dictionary<int, CheckpointData>();

    public void AddDeathLocation(Vector2 deathPosition)
    {
        deathLocations.Add(deathPosition);
    }

    public void UpdateEnemyControllerUsage(int checkpointID, int enemyType)
    {
        if (!checkpointData.ContainsKey(checkpointID))
            checkpointData[checkpointID] = new CheckpointData();

        checkpointData[checkpointID].IncreaseEnemyControllerUsage(enemyType);
    }

    public void ResetData()
    {
        deathLocations.Clear();
        foreach (var checkpoint in checkpointData.Values)
        {
            checkpoint.ResetUsage();
        }
    }
}

[System.Serializable]
public class CheckpointData
{
    private Dictionary<int, int> enemyControllerUsage = new Dictionary<int, int>();

    public void IncreaseEnemyControllerUsage(int enemyType)
    {
        if (!enemyControllerUsage.ContainsKey(enemyType))
            enemyControllerUsage[enemyType] = 0;

        enemyControllerUsage[enemyType]++;
    }

    public void ResetUsage()
    {
        enemyControllerUsage.Clear();
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