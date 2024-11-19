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

    public GameObject pauseMenu;

    [HideInInspector]
    public bool isPaused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartFromLastCheckpoint()
    {
        Time.timeScale = 1f;
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
        Time.timeScale = 1f;
        LevelManager.Instance.Restart();
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        LevelManager.Instance.LoadMainMenu();
    }

    public void NextLeevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}
