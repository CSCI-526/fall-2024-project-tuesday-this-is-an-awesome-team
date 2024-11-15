using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*This class contorls shooting enemy. It uses flying movement method and has the ability to shoot. Fire point and muzzle can rotate around the shooting enemy based on the mouse position to shoot in different directions and shoot a projectile by pressing the "space" key*/
public class ShootingEnemyController : EnemyController
{
    public GameObject projectilePrefab;
    public GameObject playerPrefab;
    public Transform firePoint;
    public Transform muzzle;
    public float fireRate = 50f; 
    private ShootingControl shootingControl;
    private PlayerController playerController;
    private bool isCooldown = false;
    public Text countdownText;

    public GameObject timerPrefab;
    private GameObject timerInstance;

    private void Start()
    {
        countdownText = UIManager.Instance.countdownText;
        playerController = GetComponent<PlayerController>();
        shootingControl = gameObject.AddComponent<ShootingControl>();
        shootingControl.init(projectilePrefab, firePoint, fireRate, false);

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

    private void Update()
    {
        if (!isCooldown) {
            RotateTowardsMouse();
            if (Input.GetKeyDown(KeyCode.Space) && shootingControl.CanShoot())
                HandleShooting();
        }
    }

    private void HandleShooting()
    {
        Vector2 shootDirection = firePoint.right; 
        shootingControl.Shoot(shootDirection, gameObject);
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 directionToMouse = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        muzzle.position = transform.position + directionToMouse * 0.5f;
        firePoint.position = transform.position + directionToMouse * 1f;
        muzzle.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
