using UnityEngine;
/*This class deals with FlyingMovement control*/
public class FlyingMovementController : MovementControllerBase
{
    public void Move(Transform transform, float moveSpeed, float turnSpeed)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }
}
