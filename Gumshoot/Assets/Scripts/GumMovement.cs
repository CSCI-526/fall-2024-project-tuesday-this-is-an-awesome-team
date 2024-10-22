using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GumMovement : MonoBehaviour
{
    enum GumState
    {
        Extending,
        Retracting,
        PullingPlayer,
        PullingObject,
        PullingEnemy
    }

    // Initialization
    private PlayerController owner;
    public GameObject StringGroupPrefab;
    public GameObject StringPrefab;
    public float stringSpriteLength = 0.08f;
    public float playerRadius = 0.325f;
    private Vector3 direction;

    private GumState state = GumState.Extending;
    private float dist = 0.0f; // Distance from starting point
    private GameObject StringGroupInstance;
    private bool isComplete = false;

    // String drawing
    private readonly Stack<GameObject> stringList = new(); // First index is closest to hook

    private Rigidbody2D rb;
    private Collider2D col;

    public void Initialize(PlayerController newOwner, Vector3 direction)
    {
        owner = newOwner;
        this.direction = direction;
        transform.localPosition += direction * 0.5f;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, -direction);
        StringGroupInstance = Instantiate(StringGroupPrefab, transform);

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        /*if (owner.PulledObject)
        {
            owner.PulledObject.GetComponent<Collider2D>().enabled = false;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GumState.Extending:
                // Extend the gum string
                if (dist < owner.maxDist)
                {
                    dist += Time.deltaTime * owner.extractSpeed;
                    //transform.localPosition += (owner.extractSpeed * Time.deltaTime * direction);
                    rb.velocity = (owner.extractSpeed * direction + (Vector3)owner.rb.velocity);
                }
                // If the gum string has reached max length, start retracting
                else
                {
                    //dist -= playerRadius;
                    rb.velocity = Vector2.zero;
                    state = GumState.Retracting;
                    col.enabled = false;
                }
                break;
            case GumState.PullingEnemy:
            case GumState.PullingObject:
                // Since the player could be moving, manually calculate the line every frame
                direction = (owner.PullContactInstance.transform.position - owner.transform.position).normalized;
                dist = (owner.PullContactInstance.transform.position - owner.transform.position).magnitude;
                owner.PulledObject.Move(owner.extractSpeed * -direction + (Vector3)owner.rb.velocity);
                goto case GumState.Retracting;
            case GumState.Retracting:
                // Moves the gum string toward the player
                if (dist > 0.8f)
                {
                    dist -= Time.deltaTime * owner.retractSpeed;
                    //transform.localPosition -= (owner.retractSpeed * Time.deltaTime * direction);
                    rb.velocity = (owner.extractSpeed * -direction + (Vector3)owner.rb.velocity);
                }
                // If the gum string has reached the player, stop all gum movement
                else
                {
                    isComplete = true;
                }
                break;
            case GumState.PullingPlayer:
                // Since the surface could be moving, manually calculate the line every frame
                direction = (owner.SurfaceContactInstance.transform.position - owner.transform.position).normalized;
                dist = (owner.SurfaceContactInstance.transform.position - owner.transform.position).magnitude;

                // Pull the player toward the gum
                if (dist > 0.8f)
                {
                    dist -= Time.deltaTime * owner.retractSpeed;
                    owner.rb.velocity = (owner.retractSpeed * direction);
                    if (owner.PulledObject != null)
                    {
                        owner.PulledObject.Move(owner.extractSpeed * direction);
                    }
                }
                // If the player has reached the gum string, stop all gum movement
                else
                {
                    owner.stuckToSurface = true;
                    isComplete = true;
                }
                break;
        }

        if (isComplete)
        {
            /*if (owner.PulledObject)
            {
                owner.PulledObject.GetComponent<Collider2D>().enabled = true;
            }*/

            // Detach the pulled object from the gum and attach it to the player instead
            if (state == GumState.PullingObject)
            {
                transform.DetachChildren();
                //owner.PulledObject.GetComponent<FixedJoint2D>().enabled = true;
                //owner.PulledObject.GetComponent<FixedJoint2D>().connectedBody = owner.rb;
                owner.PulledObject.col.enabled = true;
                owner.PulledObject.transform.parent = owner.transform;
                owner.PulledObject.Move(Vector2.zero);
                owner.PulledObject.Rotate(0f);
            }
            else if (state == GumState.PullingEnemy)
            {
                IEnemy enemyMovement = owner.PulledObject.GetComponent<IEnemy>();
                //GameObject newFlyingPlayer = Instantiate(owner.PulledObject.GetComponent<FlyingEnemyMovement>().controllerPrefab, owner.PulledObject.transform.position, Quaternion.identity);
                GameObject newPlayer = Instantiate(enemyMovement.ControllerPrefab, owner.PulledObject.transform.position, Quaternion.identity);
                newPlayer.transform.localScale = (owner.PulledObject.transform.lossyScale).Abs();

                Destroy(owner.PulledObject);
                Destroy(owner.gameObject);
                Destroy(gameObject);
                Destroy(StringGroupInstance);
                return;
            }
            else if (state == GumState.PullingPlayer)
            {
                owner.rb.position = owner.SurfaceContactInstance.transform.position - direction * 0.2f;
                owner.rb.velocity = Vector2.zero;
                owner.GetComponent<Collider2D>().enabled = true;
                if (owner.PulledObject)
                {
                    owner.PulledObject.Move(Vector2.zero);
                    owner.PulledObject.Rotate(0f);
                }
                if (owner.SurfaceContactInstance.transform.parent.GetComponent<DamageObject>() != null)
                {
                    owner.GetComponent<Health>().Damage(1, owner.SurfaceContactInstance.transform.parent.gameObject);
                }
                else if (owner.SurfaceContactInstance.transform.parent.GetComponent<EndDoor>() != null)
                {
                    owner.SurfaceContactInstance.transform.parent.GetComponent<EndDoor>().Win(owner.gameObject);
                }
            }
            owner.gumExtended = false;
            Destroy(gameObject);
            Destroy(StringGroupInstance);
            return;
        }

        DrawString();
    }

    private void OnDestroy()
    {
        Destroy(StringGroupInstance);
    }

    // Handles drawing the gum string
    private void DrawString()
    {
        float dist = (rb.position - owner.rb.position).magnitude;

        // Correct the string's rotation and position in case the player/surface could be moving
        if (state == GumState.PullingObject || state == GumState.PullingEnemy)
        {
            StringGroupInstance.transform.rotation = Quaternion.LookRotation(Vector3.forward, -direction);
            StringGroupInstance.transform.position = owner.PullContactInstance.transform.position;
        }
        else if (state == GumState.PullingPlayer)
        {
            StringGroupInstance.transform.rotation = Quaternion.LookRotation(Vector3.forward, -direction);
            StringGroupInstance.transform.position = owner.SurfaceContactInstance.transform.position;
        }

        // If the length of the string has exceeded the combined length of the string sprites, spawn another string sprite
        if (state == GumState.Extending)
        {
            while (dist > (stringList.Count - 1) * stringSpriteLength * 7.5f)
            {
                // Add chains
                GameObject StringInstance = Instantiate(StringPrefab, StringGroupInstance.transform);
                StringInstance.transform.localPosition = new Vector3(0f, (stringSpriteLength / 2f + stringSpriteLength * stringList.Count), 0f);
                stringList.Push(StringInstance);
            }
        }
        // Destroy string sprites as it retracts
        else
        {
            while (dist + playerRadius <= (stringList.Count - 1) * stringSpriteLength * 7.5f)
            {
                // Remove chains
                Destroy(stringList.Pop());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.gameObject.name);
        // Only accept gum collisions when extending
        if (state == GumState.Extending)
        {
            rb.velocity = Vector2.zero;
            // Handles pulling the player toward a surface
            if (collision.collider.gameObject.CompareTag("Surface") || collision.collider.gameObject.CompareTag("Destructible"))
            {
                GetComponent<SpriteRenderer>().enabled = false;
                owner.stuckToSurface = false;
                if (owner.SurfaceContactInstance)
                {
                    Destroy(owner.SurfaceContactInstance);
                }
                owner.SurfaceContactInstance = Instantiate(owner.contactPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, collision.GetContact(0).normal));
                owner.SurfaceContactInstance.transform.parent = collision.transform;

                // Pull the player toward the object
                state = GumState.PullingPlayer;

                // Rotate the hook to match the surface normal
                transform.DetachChildren();
                transform.rotation = Quaternion.LookRotation(Vector3.forward, collision.GetContact(0).normal);

                // Stop player movement
                transform.parent = null;
                owner.GetComponent<Rigidbody2D>().gravityScale = 0f;
                owner.GetComponent<Collider2D>().enabled = false;
                if (owner.PulledObject != null)
                {
                    owner.PulledObject.rb.gravityScale = 0f;
                }
                owner.rb.velocity = Vector2.zero;

                // Recalculate distance
                dist = Vector3.Distance(owner.transform.position, transform.position);
            }
            // Handles pulling an object toward the player
            else if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Pullable") || collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Pullable>().Move(Vector2.zero);
                collision.gameObject.GetComponent<Pullable>().Rotate(0f);

                // Do not grab objects if the player is already holding something
                if (owner.PulledObject != null)
                {
                    state = GumState.Retracting;
                    return;
                }
                Health enemyHealth = collision.gameObject.GetComponent<Health>();
                if (collision.gameObject.CompareTag("Enemy") && enemyHealth && enemyHealth.health > 0)
                {
                    state = GumState.Retracting;
                    return;
                }


                collision.gameObject.GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                if (owner.PullContactInstance)
                {
                    Destroy(owner.PullContactInstance);
                }
                owner.PullContactInstance = Instantiate(owner.contactPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, collision.GetContact(0).normal));
                owner.PullContactInstance.transform.parent = collision.transform;
                

                // Pull the object toward the player
                if (!collision.gameObject.CompareTag("Enemy"))
                {
                    state = GumState.PullingObject;
                }
                else
                {
                    state = GumState.PullingEnemy;
                }
                DamageObject damager = collision.gameObject.GetComponent<DamageObject>();
                if (damager)
                {
                    damager.instigator = owner.gameObject;
                    damager.canHurtInstigator = false;
                    damager.destroysWalls = false;
                }
                Projectile projectile = collision.gameObject.GetComponent<Projectile>();
                if (projectile)
                {
                    projectile.enabled = false;
                }

                // Attach the object onto the gum
                collision.gameObject.transform.parent = transform;
                owner.PulledObject = collision.gameObject.GetComponent<Pullable>();
                owner.PulledObject.col.enabled = false;
                owner.PulledObject.rb.gravityScale = 0f;
            }
        }
    }
}
