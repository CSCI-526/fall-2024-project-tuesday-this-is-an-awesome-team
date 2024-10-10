using UnityEngine;
/*This class deals with projectile's movement and collision*/
public class Projectile : MonoBehaviour
{
    public float speed;
    private Vector2 moveDirection;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized; 
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("projectile Collided with: " + collision.tag);
        if (collision.CompareTag("VisionCollider"))   // ignore colliding with vision collider
            return;

        if (collision.GetComponent<ShootingAction>())
            collision.GetComponent<ShootingAction>().Action();

        Destroy(gameObject); 
    }
}
