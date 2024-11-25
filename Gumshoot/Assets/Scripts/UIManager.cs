using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject winText;
    public GameObject loseText;

    public GameObject pauseMenu;

    [HideInInspector]
    public bool isPaused = false;

    public GameObject helpPanel; 
    public Transform helpContentContainer; 
    public GameObject helpRulePrefab; 

    private void Awake()
    {
        Instance = this;

        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (helpPanel.activeSelf)
            {
                helpPanel.SetActive(false);
            }
            else if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }    
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

    public void ShowHelp()
    {
        foreach (Transform child in helpContentContainer)
        {
            Destroy(child.gameObject);
        }

        string currentLevel = SceneManager.GetActiveScene().name;

        if (HelpContentManager.Instance.levelHelpContent.TryGetValue(currentLevel, out HelpContentManager.LevelHelpData helpData))
        {
            foreach (var rule in helpData.rules)
            {
                GameObject ruleInstance = Instantiate(helpRulePrefab, helpContentContainer);
                ruleInstance.transform.Find("Icon").GetComponent<Image>().sprite = rule.icon;
                ruleInstance.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = rule.text;
            }
        }
        else
        {
            Debug.LogWarning($"No help content found for level: {currentLevel}");
        }

        helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        helpPanel.SetActive(false);
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


