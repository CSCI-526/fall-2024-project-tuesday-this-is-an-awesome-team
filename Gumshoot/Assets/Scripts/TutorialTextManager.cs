using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialTextManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialTextComponent;
    public TextMeshProUGUI nextTutorialTextComponent;
    private bool hasActivated = false;
    private bool hasToggledMap = false;

    // Start is called before the first frame update
    void Start()
    {
        tutorialTextComponent.gameObject.SetActive(false);
        if (nextTutorialTextComponent != null)
        {
            nextTutorialTextComponent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level 0")
        {
            if (PlayerController.Instance != null && Input.GetKeyDown(KeyCode.E))
            {
                tutorialTextComponent.gameObject.SetActive(false);
            }

            // Show throw instruction after player grab
            if (PlayerController.Instance != null && PlayerController.Instance.gumExtended && PlayerController.Instance.PulledObject != null)
            {
                tutorialTextComponent.gameObject.SetActive(false);
                if (nextTutorialTextComponent != null)
                {
                    nextTutorialTextComponent.gameObject.SetActive(true);
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level 1")
        {

            if (PlayerController.Instance != null && PlayerController.Instance.gumExtended && PlayerController.Instance.PulledObject != null)
            {
                tutorialTextComponent.gameObject.SetActive(false);
            }

            if (FindObjectOfType<FlyingEnemyController>() != null)
            {
                if (nextTutorialTextComponent != null)
                {
                    nextTutorialTextComponent.gameObject.SetActive(true);
                }
            }
            else
            {
                if (nextTutorialTextComponent != null)
                {
                    nextTutorialTextComponent.gameObject.SetActive(false);
                }
            }

            if (hasToggledMap)
            {
                tutorialTextComponent.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                tutorialTextComponent.gameObject.SetActive(false);
                hasToggledMap = true;
            }

        }

        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (FindObjectOfType<ShootingEnemyController>() != null && gameObject.name == "TutorialTriggerSpace")
            {
                tutorialTextComponent.gameObject.SetActive(true);
            }
            else if (FindObjectOfType<ShootingEnemyController>() == null && gameObject.name == "TutorialTriggerSpace")
            {
                tutorialTextComponent.gameObject.SetActive(false);
            }
        }

        if (SceneManager.GetActiveScene().name == "Level 3")
        {
            if (FindObjectOfType<JumpingEnemyController>() != null)
            {
                tutorialTextComponent.gameObject.SetActive(false);
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            tutorialTextComponent.gameObject.SetActive(true);
            hasActivated = true;
        }
    }

}
