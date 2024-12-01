using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FlyingEnemyController : EnemyController
{
    public float moveSpeed = 5f;

    public GameObject playerPrefab;
    public GameObject timerPrefab;
    private bool isCooldown = false;

    private PlayerController playerController;
    private Rigidbody2D rb;
    private GameObject timerInstance;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        StartTimer();
        StartCoroutine(CooldownCoroutine());
    }

    void StartTimer()
    {
        timerInstance = Instantiate(timerPrefab, transform.position, Quaternion.identity);
        timerInstance.transform.SetParent(null);

        CountdownTimer countdownTimer = timerInstance.GetComponent<CountdownTimer>();
        if (countdownTimer != null)
        {
            countdownTimer.Init(transform, cooldownDuration);
        }
    }

    void OnDestroy()
    {
        if (timerInstance != null)
        {
            Destroy(timerInstance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCooldown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (playerController.stuckToSurface && (horizontalInput != 0 || verticalInput != 0))
            {
                playerController.Jump(Vector3.zero);
            }

            Vector2 moveDirection = new Vector2(horizontalInput, verticalInput);
            rb.velocity = moveSpeed * moveDirection;
            if (playerController.PulledObject)
            {
                playerController.PulledObject.Move(moveSpeed * moveDirection);
            }
        }
    }

    IEnumerator CooldownCoroutine()
    {
        float remainingTime = cooldownDuration;

        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        isCooldown = true;
        GameObject newPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        RevertPlayer(newPlayer);
        Destroy(gameObject);
    }
}
