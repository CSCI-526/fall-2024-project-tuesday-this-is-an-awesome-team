using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public GameObject gumPrefab;
    public GameObject contactPrefab;
    public static PlayerController Instance;

    [HideInInspector] public bool gumExtended = false;
    [HideInInspector] public Pullable PulledObject = null;
    [HideInInspector] public GameObject StuckSurface = null;
    [HideInInspector] public GameObject SurfaceContactInstance = null;
    [HideInInspector] public GameObject PullContactInstance = null;

    public float jumpForce = 5f;
    public float throwForce = 15f;
    public float maxDist;
    public float extractSpeed;
    public float retractSpeed;

    [HideInInspector] public bool stuckToSurface = false;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public GameObject gumInstance = null;

    public Transform aimArrow;
    private TrajectoryVisualizer trajectoryVisualizer;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        trajectoryVisualizer = GetComponent<TrajectoryVisualizer>();

        if (DataPersistanceManager.Instance && DataPersistanceManager.Instance.hasNewPos)
        {
            transform.position = DataPersistanceManager.Instance.newPos;
            DataPersistanceManager.Instance.hasNewPos = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsMouse();
        
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //if (PulledObject != null && !PulledObject.CompareTag("Enemy"))
        //{
        //    Vector3 direction = aimArrow.right;
        //    Vector2 startVelocity = direction.normalized * throwForce;
        //    //float gravityScale = PulledObject.rb.gravityScale;
        //    float gravityScale = 1.6f;
        //    trajectoryVisualizer.ShowTrajectory(aimArrow.position, startVelocity, gravityScale);
        //}
        //else
        //{
        //    trajectoryVisualizer.HideTrajectory();
        //}

        // Keep the player stuck to the surface if the surface is moving
        if (stuckToSurface && SurfaceContactInstance)
        {
            Vector3 direction = (SurfaceContactInstance.transform.position - transform.position).normalized;
            float dist = (SurfaceContactInstance.transform.position - transform.position).magnitude;
            if (dist > 1.0f)
            {
                //rb.velocity = (retractSpeed * dist * 0.5f * direction);
                transform.position += (retractSpeed * direction * Time.deltaTime);
            }
        }
        // Keep the item following the player if the item gets stuck
        if (gumInstance == null && PulledObject)
        {
            Vector3 direction = (transform.position - PulledObject.transform.position).normalized;
            float dist = (PulledObject.transform.position - transform.position).magnitude;
            if (dist > 1.6f)
            {
                PulledObject.transform.position += (retractSpeed * direction * Time.deltaTime);
            }
        }

        // Launch grappling hook on left click
        if (Input.GetKeyDown(KeyCode.Mouse0) && !gumExtended)
        {
            Vector3 direction = GetMouseForward();

            gumExtended = true;
            // Spawn gum
            gumInstance = Instantiate(gumPrefab, transform);
            gumInstance.GetComponent<GumMovement>().Initialize(this, direction);
        }

        // jumping
        //if (Input.GetKeyDown(KeyCode.E) && !gumExtended)
        //{
        //    Vector3 direction = GetMouseForward();
        //    // If extending towards the latched surface, then launch off the surface
        //    Jump(direction);
        //}

        // jumping
        if (Input.GetKey(KeyCode.E) && !gumExtended)
        {
            Debug.Log($"Current GameObject name: {gameObject.name}");
            if (gameObject.name == "JumpingPlayer(Clone)")
            {
                Vector3 direction = aimArrow.right;
                Vector2 startVelocity = direction.normalized * jumpForce;
                float gravityScale = 1.6f;
                trajectoryVisualizer.ShowTrajectory(aimArrow.position, startVelocity, gravityScale);
            }
        }
        else if (Input.GetKeyUp(KeyCode.E) && !gumExtended)
        {
            Vector3 direction = GetMouseForward();
            // If extending towards the latched surface, then launch off the surface
            Jump(direction);
            trajectoryVisualizer.HideTrajectory();
        }

        // throwing object
        //else if (Input.GetKeyDown(KeyCode.Mouse1) && !gumExtended)
        //{
        //    if (PulledObject != null)
        //    {
        //       Vector3 direction = GetMouseForward();
        //        // Disconnect the pulled object from the player
        //        //PulledObject.GetComponent<FixedJoint2D>().connectedBody = null;
        //        //PulledObject.GetComponent<FixedJoint2D>().enabled = false;
        //        PulledObject.transform.parent = null;
        //        PulledObject.rb.gravityScale = 1.6f;
        //        PulledObject.col.enabled = true;
        //        DamageObject damageObj = PulledObject.GetComponent<DamageObject>();
        //        if (damageObj)
        //        {
        //            StartCoroutine(damageObj.Launch(gameObject, 0.4f));
        //        }
        //        if (PullContactInstance)
        //        {
        //            Destroy(PullContactInstance);
        //            PullContactInstance = null;
        //        }

        //        // Launch the object in the mouse direction
        //        PulledObject.transform.position = transform.position + direction;
        //        //PulledObject.rb.AddForce(direction * throwForce);
        //        PulledObject.rb.velocity = direction * throwForce;

        //        PulledObject = null;
        //    }
        //}

        // throwing object
        else if (Input.GetKey(KeyCode.Mouse1) && !gumExtended)
        {
            if (PulledObject != null)
            {
                Vector3 direction = aimArrow.right;
                Vector2 startVelocity = direction.normalized * throwForce;
                float gravityScale = 1.6f;
                trajectoryVisualizer.ShowTrajectory(aimArrow.position, startVelocity, gravityScale);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && !gumExtended)
        {
            if (PulledObject != null)
            {
                Vector3 direction = GetMouseForward();
                // Disconnect the pulled object from the player
                //PulledObject.GetComponent<FixedJoint2D>().connectedBody = null;
                //PulledObject.GetComponent<FixedJoint2D>().enabled = false;
                PulledObject.transform.parent = null;
                PulledObject.rb.gravityScale = 1.6f;
                PulledObject.col.enabled = true;
                DamageObject damageObj = PulledObject.GetComponent<DamageObject>();
                if (damageObj)
                {
                    StartCoroutine(damageObj.Launch(gameObject, 0.4f));
                }
                if (PullContactInstance)
                {
                    Destroy(PullContactInstance);
                    PullContactInstance = null;
                }

                // Launch the object in the mouse direction
                PulledObject.transform.position = transform.position + direction;
                //PulledObject.rb.AddForce(direction * throwForce);
                PulledObject.rb.velocity = direction * throwForce;

                PulledObject = null;

                trajectoryVisualizer.HideTrajectory();
            }
        }
        else
        {
            trajectoryVisualizer.HideTrajectory();
        }

    }

    public void Jump(Vector3 direction)
    {
        if (gumExtended)
        {
            return;
        }

        // If not stuck to a surface and not grounded, do not jump
        if (!stuckToSurface && rb.velocity.y != 0)
        {
            return;
        }

        stuckToSurface = false;
        StuckSurface = null;
        if (GetComponent<FlyingEnemyController>() == null)
        {
            rb.gravityScale = 1.6f;
            rb.velocity = direction * jumpForce;

            //rb.AddForce(direction * jumpForce);
            //if (PulledObject != null)
            //{
            //    //PulledObject.rb.gravityScale = 1.6f;
            //    //PulledObject.rb.AddForce(direction * jumpForce);
            //}
        }

        if (SurfaceContactInstance)
        {
            Destroy(SurfaceContactInstance);
            SurfaceContactInstance = null;
        }
    }

    public void Die()
    {
        StuckSurface = null;
        if (PulledObject != null)
        {
            // Disconnect the pulled object from the player
            PulledObject.transform.parent = null;
            PulledObject.col.enabled = true;
            PulledObject.rb.gravityScale = 1.6f;

            PulledObject = null;
        }

        if (PullContactInstance)
        {
            Destroy(PullContactInstance);
        }
        if (SurfaceContactInstance)
        {
            Destroy(SurfaceContactInstance);
        }
        if (gumInstance)
        {
            Destroy(gumInstance);
        }
    }

    Vector3 GetMouseForward()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return (mousePos - transform.position).normalized;
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 directionToMouse = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        aimArrow.position = transform.position + directionToMouse * 1f;
        aimArrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

}
