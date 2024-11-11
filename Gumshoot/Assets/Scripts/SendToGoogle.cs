using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.EventSystems.EventTrigger;

public class SendToGoogle : MonoBehaviour
{
    public static SendToGoogle Instance;
    [SerializeField] private string URL;
    private long _sessionID;
    private string _allDeathLocations;
    private string _timesUseEnemyAbility;
    private string _allTimeSpent;
    private string _allTriggerCounts;
    public bool enable = false;

    private void Awake()
    {
        Instance = this;
        _sessionID = DateTime.Now.Ticks;
    }

    public void Send()
    {
        _allDeathLocations = GetAllDeathLocations();
        _timesUseEnemyAbility = GetAllEnemyControllerUsage();
        _allTimeSpent = GetAllTimeSpent();
        _allTriggerCounts = GetAllTriggerStatuses();

        string platform = Application.platform == RuntimePlatform.WebGLPlayer ? "WebGL" : "UnityEditor";
        string sessionWithPlatform = $"{_sessionID}|{platform}";

        StartCoroutine(Post(sessionWithPlatform, _allDeathLocations, _timesUseEnemyAbility, _allTimeSpent, _allTriggerCounts));
    }

    private string GetAllDeathLocations()
    {
        List<string> deathLocations = new List<string>();

        foreach (var levelData in LevelManager.levelsData)
        {
            string levelName = levelData.Key;
            if (levelData.Value.deathLocations != null && levelData.Value.deathLocations.Count > 0)
            {
                string locations = string.Join(", ", levelData.Value.deathLocations.ConvertAll(v => $"{v.x}, {v.y}"));
                Debug.Log($"Death locations for {levelName}: {locations}");
                deathLocations.Add($"{levelName}: {locations}");
            }
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
                if (checkpoint.Value.HasEnemyUsageData())
                {
                    string usage = string.Join(", ", checkpoint.Value.GetEnemyControllerUsage());
                    usageData.Add($"{levelName} Checkpoint {checkpointID}: {usage}");
                }
            }
        }

        return string.Join(" | ", usageData);
    }

    private string GetAllTimeSpent()
    {
        List<string> timeSpentData = new List<string>();

        foreach (var levelData in LevelManager.levelsData)
        {
            string levelName = levelData.Key;

            foreach (var checkpoint in levelData.Value.checkpointData)
            {
                int checkpointID = checkpoint.Key;
                if (checkpoint.Value.HasTimeSpentData())
                {
                    string times = string.Join(", ", checkpoint.Value.timeSpentList);
                    timeSpentData.Add($"{levelName} Checkpoint {checkpointID}: Time Spent = [{times}]");
                }
            }
        }
        string result = string.Join(" | ", timeSpentData);
        Debug.Log("Final result for all time spents: " + result);
        return result;
    }

    private string GetAllTriggerStatuses()
    {
        List<string> triggerStatusData = new List<string>();

        foreach (var levelData in LevelManager.levelsData)
        {
            string levelName = levelData.Key;

            foreach (var checkpoint in levelData.Value.checkpointData)
            {
                int checkpointID = checkpoint.Key;
                if (checkpoint.Value.HasTriggerData())
                {
                    triggerStatusData.Add($"{levelName} Checkpoint {checkpointID}");
                }
            }
        }

        return string.Join(" | ", triggerStatusData);
    }


    private IEnumerator Post(string sessionID, string allDeathLocations, string timesUseEnemyAbility, string allTimeSpent, string allTriggerCounts)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.835694260", sessionID);
        form.AddField("entry.356227542", allTriggerCounts);
        form.AddField("entry.1741040447", allDeathLocations);
        form.AddField("entry.1026778936", timesUseEnemyAbility);
        form.AddField("entry.2067514501", allTimeSpent);

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
