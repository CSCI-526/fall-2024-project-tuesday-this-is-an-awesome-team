using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
/*This class deals with basic enemy controls, including making virtual camera following the enemy, 
 * starting and displaying the countdown for the player to gain the enemy's abilities, 
 * and moving the enemy using a movement strategy. */
public class EnemyControllerBase : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 50f;
    public GameObject playerPrefab; 
    protected MovementControllerBase movementStrategy;
    protected bool isCooldown = false; 
    [SerializeField] protected float cooldownDuration = 12f;
    public Text countdownText; 

    protected virtual void Start()
    {
        countdownText = GameObject.Find("Canvas").GetComponentInChildren<Text>();
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        StartCoroutine(CooldownCoroutine());
    }

    protected virtual void Update()
    {
        if (!isCooldown)
            movementStrategy?.Move(transform, moveSpeed, turnSpeed);
    }
    protected IEnumerator CooldownCoroutine()
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
