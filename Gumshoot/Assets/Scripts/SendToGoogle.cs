using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL;
    private long _sessionID;
    private string _deathsPerCheckpoint;
    private string _deathLocationsLevel0;
    private string _deathLocationsLevel1;
    private string _deathLocationsLevelMain;
    private string _checkpointOrder;
    private int _timesUseEnemyAbility;

    private void Awake()
    {
        _sessionID = DateTime.Now.Ticks;

    }

    private void OnApplicationQuit()
    {
        Send();
    }

    public void Send()
    {
        _deathsPerCheckpoint = string.Join(", ", LevelManager.deathPerCheckpoint);
        _deathLocationsLevel0 = string.Join(", ", LevelManager.deathLocationListLevel0.ConvertAll(v => $"{v.x}, {v.y}"));
        _deathLocationsLevel1 = string.Join(", ", LevelManager.deathLocationListLevel1.ConvertAll(v => $"{v.x}, {v.y}"));
        _deathLocationsLevelMain = string.Join(", ", LevelManager.deathLocationListLevelMain.ConvertAll(v => $"{v.x}, {v.y}"));
        _checkpointOrder = "01234";
        _timesUseEnemyAbility = 2;

        StartCoroutine(Post(_sessionID.ToString(), _deathsPerCheckpoint, _deathLocationsLevel0, _deathLocationsLevel1, _deathLocationsLevelMain, _checkpointOrder, _timesUseEnemyAbility.ToString()));
    }


    private IEnumerator Post(string sessionID, string _deathsPerCheckpoint, string _deathLocationsLevel0, string _deathLocationsLevel1, string _deathLocationsLevelMain, string _checkpointOrder, string _timesUseEnemyAbility)
    {
        string _allDeathLocations = $"{_deathLocationsLevel0} | {_deathLocationsLevel1} | {_deathLocationsLevelMain}";
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.835694260", sessionID);
        form.AddField("entry.356227542", _deathsPerCheckpoint);
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
