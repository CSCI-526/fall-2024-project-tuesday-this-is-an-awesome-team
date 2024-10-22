using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject winText;
    public GameObject loseText;
    public Text countdownText;

    private void Awake()
    {
        Instance = this;
    }

    public void RestartFromLastCheckpoint()
    {
        LevelManager.Instance.Respawn();
    }

    public void Reset()
    {
        Debug.Log("Reset pressed");
        if (DataPersistanceManager.Instance)
        {
            Destroy(DataPersistanceManager.Instance.gameObject);
            DataPersistanceManager.Instance = null;
        }
        LevelManager.Instance.Restart();
    }

    public void ReturnToTitle()
    {
        LevelManager.Instance.LoadMainMenu();
    }

    public void NextLeevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}
