using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL;
    private long _sessionID;
    private string _deathsPerCheckpoint;
    private Vector3 _deathLocation;
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

        _deathLocation = new Vector3(0,0,0);
        _checkpointOrder = "01234";
        _timesUseEnemyAbility = 2;

        StartCoroutine(Post(_sessionID.ToString(), _deathsPerCheckpoint, _deathLocation.ToString(), _checkpointOrder, _timesUseEnemyAbility.ToString()));
    }


    private IEnumerator Post(string sessionID, string _deathsPerCheckpoint, string _deathLocation, string _checkpointOrder, string _timesUseEnemyAbility)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.835694260", sessionID);
        form.AddField("entry.356227542", _deathsPerCheckpoint);
        form.AddField("entry.1741040447", _deathLocation);
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
