using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class JumpingEnemyController : MonoBehaviour
{
    public GameObject playerPrefab;
    private bool isCooldown = false;
    [SerializeField] private float cooldownDuration = 12f;
    public Text countdownText;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        countdownText = GameObject.Find("Canvas").GetComponentInChildren<Text>();
        playerController = GetComponent<PlayerController>();

        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        JumpingEnemyMovement.player = transform;
        StartCoroutine(CooldownCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCooldown)
        {
            playerController.jumpForce = 500f;
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
        JumpingEnemyMovement.player = newPlayer.transform;
        Destroy(gameObject);
    }
}
