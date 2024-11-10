using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    public static SendToGoogle Instance;
    [SerializeField] private string URL;
    private long _sessionID;
    private string _allDeathLocations;
    private string _timesUseEnemyAbility;
    public bool enable = false;

    private void Awake()
    {
        Instance = this;
        _sessionID = DateTime.Now.Ticks;
    }

    public void Send()
    {
        // 获取所有关卡的死亡位置和敌人控制器使用数据
        _allDeathLocations = GetAllDeathLocations();
        _timesUseEnemyAbility = GetAllEnemyControllerUsage();

        string platform = Application.platform == RuntimePlatform.WebGLPlayer ? "WebGL" : "UnityEditor";
        string sessionWithPlatform = $"{_sessionID}|{platform}";

        StartCoroutine(Post(sessionWithPlatform, _allDeathLocations, _timesUseEnemyAbility));
    }

    private string GetAllDeathLocations()
    {
        // 检查 LevelManager.Instance 是否为 null
        if (LevelManager.Instance == null)
        {
            Debug.LogError("LevelManager.Instance is null!");
            return string.Empty;
        }

        // 检查 levelsData 是否为空或未初始化
        if (LevelManager.levelsData == null || LevelManager.levelsData.Count == 0)
        {
            Debug.LogError("LevelManager.Instance.levelsData is empty or null!");
            return string.Empty;
        }

        List<string> deathLocations = new List<string>();

        Debug.Log("Starting to gather death locations for each level...");

        foreach (var levelData in LevelManager.levelsData)
        {
            string levelName = levelData.Key;
            Debug.Log($"Processing level: {levelName}");

            // 转换死亡位置为字符串
            string locations = string.Join(", ", levelData.Value.deathLocations.ConvertAll(v => $"{v.x}, {v.y}"));
            Debug.Log($"Death locations for {levelName}: {locations}");

            deathLocations.Add($"{levelName}: {locations}");
        }

        string result = string.Join(" | ", deathLocations);
        Debug.Log("Final result for all death locations: " + result);

        return result;
    }

    private string GetAllEnemyControllerUsage()
    {
        List<string> usageData = new List<string>();

        foreach (var levelData in LevelManager.levelsData)
        {
            string levelName = levelData.Key;
            foreach (var checkpoint in levelData.Value.checkpointData)
            {
                int checkpointID = checkpoint.Key;
                string usage = string.Join(", ", checkpoint.Value.GetEnemyControllerUsage());
                usageData.Add($"{levelName} Checkpoint {checkpointID}: {usage}");
            }
        }

        return string.Join(" | ", usageData);
    }

    private IEnumerator Post(string sessionID, string allDeathLocations, string timesUseEnemyAbility)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.835694260", sessionID);
        form.AddField("entry.1741040447", allDeathLocations);
        form.AddField("entry.2067514501", timesUseEnemyAbility);

        // Send responses and verify result
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
