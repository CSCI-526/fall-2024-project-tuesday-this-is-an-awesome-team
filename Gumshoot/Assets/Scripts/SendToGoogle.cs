using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    public static SendToGoogle Instance;
    [SerializeField] private string URL;
    private long _sessionID;
    private string _deathsPerCheckpoint;
    private string _deathLocationsLevel0;
    private string _deathLocationsLevel1;
    private string _deathLocationsLevel2;
    private string _deathLocationsLevel3;
    private string _deathLocationsLevelMain;
    private string _checkpointOrder;
    private string _timesUseEnemyAbility;
    public bool enable = false;

    private void Awake()
    {
        Instance = this;
        _sessionID = DateTime.Now.Ticks;
    }

    public void Send()
    {
        //_deathsPerCheckpoint = string.Join(", ", LevelManager.deathPerCheckpoint);
        _deathLocationsLevel0 = string.Join(", ", LevelManager.deathLocationListLevel0.ConvertAll(v => $"{v.x}, {v.y}"));
        _deathLocationsLevel1 = string.Join(", ", LevelManager.deathLocationListLevel1.ConvertAll(v => $"{v.x}, {v.y}"));
        _deathLocationsLevel2 = string.Join(", ", LevelManager.deathLocationListLevel2.ConvertAll(v => $"{v.x}, {v.y}"));
        _deathLocationsLevel3 = string.Join(", ", LevelManager.deathLocationListLevel3.ConvertAll(v => $"{v.x}, {v.y}"));
        _deathLocationsLevelMain = string.Join(", ", LevelManager.deathLocationListLevelMain.ConvertAll(v => $"{v.x}, {v.y}"));
        _checkpointOrder = "01234";
        _timesUseEnemyAbility = string.Join(", ", LevelManager.EnemyControllerUse);
        string platform = Application.platform == RuntimePlatform.WebGLPlayer ? "WebGL" : "UnityEditor";
        string sessionWithPlatform = $"{_sessionID}|{platform}";
        StartCoroutine(Post(sessionWithPlatform, _deathLocationsLevel0, _deathLocationsLevel1, _deathLocationsLevel2, _deathLocationsLevel3, _deathLocationsLevelMain, _checkpointOrder, _timesUseEnemyAbility));
    }


    private IEnumerator Post(string sessionID, string _deathLocationsLevel0, string _deathLocationsLevel1, string _deathLocationsLevel2, string _deathLocationsLevel3, string _deathLocationsLevelMain, string _checkpointOrder, string _timesUseEnemyAbility)
    {
        string _allDeathLocations = $"{_deathLocationsLevel0} | {_deathLocationsLevel1} | {_deathLocationsLevel2} | {_deathLocationsLevel3} | {_deathLocationsLevelMain}";
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.835694260", sessionID);
        //form.AddField("entry.356227542", _deathsPerCheckpoint);
        form.AddField("entry.1741040447", _allDeathLocations);
        form.AddField("entry.1026778936", _checkpointOrder);
        form.AddField("entry.2067514501", _timesUseEnemyAbility);

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
