using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FlyingEnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public GameObject playerPrefab;
    private bool isCooldown = false;
    [SerializeField] private float cooldownDuration = 12f;
    public Text countdownText;

    private PlayerController playerController;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        countdownText = UIManager.Instance.countdownText;
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        StartCoroutine(CooldownCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCooldown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            if (horizontalInput != 0 || verticalInput != 0)
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
            countdownText.text = remainingTime.ToString("F1") + "s";
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        countdownText.text = "";

        isCooldown = true;
        GameObject newPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = newPlayer.transform;
        Destroy(gameObject);
    }
}
