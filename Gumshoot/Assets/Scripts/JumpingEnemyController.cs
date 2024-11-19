using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class JumpingEnemyController : EnemyController
{
    public GameObject playerPrefab;
    public GameObject timerPrefab;
    private bool isCooldown = false;

    private PlayerController playerController;
    private GameObject timerInstance;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

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
