using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialTextManager : MonoBehaviour
{
    public GameObject tutorialGroup;
    public Image tutorialImage;
    public TextMeshProUGUI tutorialTextComponent;

    public GameObject nextTutorialGroup;
    public Image nextTutorialImage;
    public TextMeshProUGUI nextTutorialTextComponent;

    private bool hasActivated = false;
    private bool hasToggledMap = false;

    private bool hasThrowed = false;

    public RectTransform arrowIcon;


    // Flying take control only be triggered once
    private bool tutorialShown2 = false;

    // Flying WASD control only be triggered once
    // Shooting SPACE only be triggered once
    // Jumping E increased only be triggered once
    private bool tutorialShown = false;

    private float flashTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (tutorialGroup != null)
        {
            tutorialGroup.SetActive(false);
        }

        if (nextTutorialGroup != null)
        {
            nextTutorialGroup.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            if (gameObject.name == "TutorialTriggerStick")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    tutorialGroup.gameObject.SetActive(false);
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (gameObject.name == "TutorialTriggerStick")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    tutorialGroup.gameObject.SetActive(false);
                }
            }

            if (gameObject.name == "TutorialTriggerJump")
            {
                if (tutorialGroup != null & tutorialGroup.activeSelf)
                {
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = 0f;
                    //Vector2 direction = (Vector2)(mouseWorldPosition - arrowIcon.position);
                    //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    //arrowIcon.rotation = Quaternion.Euler(0, 0, angle + 45f);
                }

                if (PlayerController.Instance != null && Input.GetKeyDown(KeyCode.E))
                {
                    tutorialGroup.gameObject.SetActive(false);
                }
            }

            if (gameObject.name == "TutorialTriggerGrabThrow")
            {
                if (PlayerController.Instance != null && PlayerController.Instance.gumExtended && PlayerController.Instance.PulledObject != null)
                {
                    tutorialGroup.gameObject.SetActive(false);

                    if (!hasThrowed)
                    {
                        nextTutorialGroup.gameObject.SetActive(true);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (!tutorialGroup.activeSelf && nextTutorialGroup.activeSelf)
                    {
                        nextTutorialGroup.gameObject.SetActive(false);

                        hasThrowed = true;
                    }
                }
            }

            if (gameObject.name == "TutorialTriggerButton")
            {
                Button button = FindObjectOfType<Button>();
                if (button.IsPressed)
                {
                    tutorialGroup.SetActive(false);
                }
            }



        }

        // Flying
        if (SceneManager.GetActiveScene().name == "Level 3")
        {
            
            if (gameObject.name == "TutorialTriggerControl")
            {
                FlyingEnemyMovement flyingEnemy = GameObject.Find("FlyingEnemy")?.GetComponent<FlyingEnemyMovement>();

                if (flyingEnemy != null && flyingEnemy.GetComponent<SpriteRenderer>().color == Color.red && arrowIcon != null && arrowIcon.gameObject.activeSelf)
                {
                    SetPositionWithOffset(arrowIcon.gameObject, flyingEnemy.transform, new Vector3(0, 1.25f, 0));
                }
                else
                {
                    arrowIcon.gameObject.SetActive(false);
                }

                if (!tutorialShown2 && PlayerController.Instance != null && PlayerController.Instance.gumExtended && PlayerController.Instance.PulledObject != null)
                {
                    tutorialGroup.gameObject.SetActive(false);
                    arrowIcon.gameObject.SetActive(true);
                }

                if (flyingEnemy != null && flyingEnemy.GetComponent<SpriteRenderer>().color == Color.yellow)
                {
                    SetPositionWithOffset(nextTutorialGroup, flyingEnemy.transform, Vector3.right * 1.0f);
                    nextTutorialGroup.gameObject.SetActive(true);
                    arrowIcon.gameObject.SetActive(false);
                }
                else
                {
                    nextTutorialGroup.gameObject.SetActive(false);
                    //tutorialShown2 = true;
                }

            }

            if (gameObject.name == "TutorialTriggerControl2")
            {
                FlyingEnemyMovement flyingEnemy = GameObject.Find("FlyingEnemy2")?.GetComponent<FlyingEnemyMovement>();
                if (!tutorialShown2 && PlayerController.Instance != null && PlayerController.Instance.gumExtended && PlayerController.Instance.PulledObject != null)
                {
                    tutorialGroup.gameObject.SetActive(false);
                }

                if (flyingEnemy != null && flyingEnemy.GetComponent<SpriteRenderer>().color == Color.yellow)
                {
                    SetPositionWithOffset(nextTutorialGroup, flyingEnemy.transform, Vector3.right * 1.0f);
                    nextTutorialGroup.gameObject.SetActive(true);
                }
                else
                {
                    nextTutorialGroup.gameObject.SetActive(false);
                    //tutorialShown2 = true;
                }

            }

            if (gameObject.name == "TutorialTriggerFly")
            {
                FlyingEnemyController flyingPlayer = FindObjectOfType<FlyingEnemyController>();
                if (flyingPlayer && !tutorialShown)
                {
                    SetPositionWithOffset(tutorialGroup, flyingPlayer.transform, Vector3.up * 1.0f);
                    tutorialGroup.gameObject.SetActive(true);
                    
                }
                else
                {
                    tutorialGroup.gameObject.SetActive(false);
                }

                if (!tutorialShown && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
                {
                    tutorialGroup.gameObject.SetActive(false);
                    tutorialShown = true;
                }
            }

            if (gameObject.name == "TutorialTriggerMap" || gameObject.name == "TutorialTriggerMap2")
            {
                if (tutorialGroup.gameObject.activeSelf)
                {
                    FlashTutorialImage();
                }
                if (Input.GetKeyDown(KeyCode.M) && !hasToggledMap)
                {
                    tutorialGroup.gameObject.SetActive(false);
                    hasToggledMap = true;
                }
            }

        }

        // Shooting
        if (SceneManager.GetActiveScene().name == "Level 4")
        {
            if (gameObject.name == "TutorialTriggerMap" || gameObject.name == "TutorialTriggerMap2")
            {
                if (tutorialGroup.gameObject.activeSelf)
                {
                    FlashTutorialImage();
                }
                if (Input.GetKeyDown(KeyCode.M) && !hasToggledMap)
                {
                    tutorialGroup.gameObject.SetActive(false);
                    hasToggledMap = true;
                }
            }

            ShootingEnemyController shootingPlayer = FindObjectOfType<ShootingEnemyController>();
            if (gameObject.name == "TutorialTriggerSpace")
            {
                if (shootingPlayer && !tutorialShown)
                {
                    SetPositionWithOffset(tutorialGroup, shootingPlayer.transform, Vector3.right * 1.0f);
                    tutorialGroup.gameObject.SetActive(true);
                }
                else
                {
                    tutorialGroup.gameObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.Space) && !tutorialShown)
                {
                    tutorialGroup.gameObject.SetActive(false);
                    tutorialShown = true;
                }
            }

            if (gameObject.name == "TutorialTriggerBullet" || gameObject.name == "TutorialTriggerBullet2")
            {
                if (PlayerController.Instance.PulledObject != null)
                {
                    tutorialGroup.gameObject.SetActive(false);
                }
                else
                {
                    GameObject projectile = GameObject.Find("Projectile2");
                    if (projectile != null && arrowIcon != null)
                    {
                        if (gameObject.name == "TutorialTriggerBullet")
                        {
                            SetPositionWithOffset(arrowIcon.gameObject, projectile.transform, new Vector3(0.75f, -0.75f, 0));
                        }
                        else
                        {
                            SetPositionWithOffset(arrowIcon.gameObject, projectile.transform, new Vector3(-0.75f, -0.75f, 0));
                        }
                        arrowIcon.gameObject.SetActive(true);
                    }
                    else if (arrowIcon != null)
                    {
                        arrowIcon.gameObject.SetActive(false);
                    }
                }
            }
            
        }

        // Jumping
        if (SceneManager.GetActiveScene().name == "Level 5")
        {
            if (gameObject.name == "TutorialTriggerJump")
            {
                if (tutorialGroup != null & tutorialGroup.activeSelf)
                {
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = 0f;
                    //Vector2 direction = (Vector2)(mouseWorldPosition - arrowIcon.position);
                    //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    //arrowIcon.rotation = Quaternion.Euler(0, 0, angle + 45f);
                }

                if (PlayerController.Instance != null && Input.GetKeyDown(KeyCode.E))
                {
                    tutorialGroup.gameObject.SetActive(false);
                }
            }

            JumpingEnemyController jumpingPlayer = FindObjectOfType<JumpingEnemyController>();
            if (gameObject.name == "TutorialTriggerJumping")
            {
                if (jumpingPlayer && !tutorialShown)
                {
                    SetPositionWithOffset(tutorialGroup, jumpingPlayer.transform, Vector3.right * 1.0f);
                    tutorialGroup.gameObject.SetActive(true);
                }
                else
                {
                    tutorialGroup.gameObject.SetActive(false);
                }

                if (jumpingPlayer && Input.GetKeyDown(KeyCode.E) && !tutorialShown)
                {
                    tutorialGroup.gameObject.SetActive(false);
                    tutorialShown = true;
                }
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated && !hasToggledMap)
        {
            tutorialGroup.gameObject.SetActive(true);
            hasActivated = true;
        }
    }

    private void SetPositionWithOffset(GameObject target, Transform source, Vector3 offset)
    {
        if (target != null && source != null)
        {
            Vector3 newPosition = source.position + offset;
            target.transform.position = newPosition;
        }
    }

    private void FlashTutorialImage()
    {
        flashTimer += Time.deltaTime;

        if (flashTimer >= 0.5f)
        {
            flashTimer = 0f;

            Color currentColor = tutorialImage.color;
            tutorialImage.color = new Color(
                currentColor.r,
                currentColor.g,
                currentColor.b,
                currentColor.a == 1f ? 0.5f : 1f
            );
        }
    }
}
