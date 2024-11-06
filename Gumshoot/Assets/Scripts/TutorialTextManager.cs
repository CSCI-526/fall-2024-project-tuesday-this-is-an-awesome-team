using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialTextManager : MonoBehaviour
{
    public Image tutorialImage;
    public TextMeshProUGUI tutorialTextComponent;

    public Image nextTutorialImage;
    public TextMeshProUGUI nextTutorialTextComponent;

    private bool hasActivated = false;
    private bool hasToggledMap = false;
    private bool hasThrowed = false;


    // Start is called before the first frame update
    void Start()
    {
        if (tutorialImage != null)
        {
            tutorialImage.gameObject.SetActive(false);
        }

        if (tutorialTextComponent != null)
        {
            tutorialTextComponent.gameObject.SetActive(false);
        }

        if (nextTutorialImage != null)
        {
            nextTutorialImage.gameObject.SetActive(false);
        }

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
            if (gameObject.name == "TutorialTriggerStick")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    tutorialImage.gameObject.SetActive(false);
                    tutorialTextComponent.gameObject.SetActive(false);
                }
            }

            if (gameObject.name == "TutorialTriggerJump")
            {
                if (PlayerController.Instance != null && Input.GetKeyDown(KeyCode.E))
                {
                    tutorialImage.gameObject.SetActive(false);
                    tutorialTextComponent.gameObject.SetActive(false);
                }
            }

            if (gameObject.name == "TutorialTriggerGrabThrow")
            {
                if (PlayerController.Instance != null && PlayerController.Instance.gumExtended && PlayerController.Instance.PulledObject != null)
                {
                    tutorialImage.gameObject.SetActive(false);
                    tutorialTextComponent.gameObject.SetActive(false);

                    if (!hasThrowed)
                    {
                        nextTutorialImage.gameObject.SetActive(true);
                        nextTutorialTextComponent.gameObject.SetActive(true);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    nextTutorialImage.gameObject.SetActive(false);
                    nextTutorialTextComponent.gameObject.SetActive(false);

                    hasThrowed = true;
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            if (gameObject.name == "TutorialTriggerFly")
            {
                if (PlayerController.Instance != null && PlayerController.Instance.gumExtended && PlayerController.Instance.PulledObject != null)
                {

                    {
                        tutorialTextComponent.gameObject.SetActive(false);
                    }
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
            }

            if (gameObject.name == "TutorialTriggerMap" || gameObject.name == "TutorialTriggerMap2")
            {
                if (Input.GetKeyDown(KeyCode.M) && !hasToggledMap)
                {
                    tutorialTextComponent.gameObject.SetActive(false);
                    hasToggledMap = true;
                }
            }

        }

        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (gameObject.name == "TutorialTriggerSpace")
            {
                if (FindObjectOfType<ShootingEnemyController>() != null)
                {
                    tutorialTextComponent.gameObject.SetActive(true);
                }
                else if (FindObjectOfType<ShootingEnemyController>() == null)
                {
                    tutorialTextComponent.gameObject.SetActive(false);
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level 3")
        {
            if (gameObject.name == "TutorialTriggerJumping")
            {
                if (FindObjectOfType<JumpingEnemyController>() != null)
                {
                    tutorialTextComponent.gameObject.SetActive(false);
                }
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated && !hasToggledMap)
        {
            tutorialImage.gameObject.SetActive(true);
            tutorialTextComponent.gameObject.SetActive(true);
            hasActivated = true;
        }
    }

}
