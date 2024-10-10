using UnityEngine;
/*This class deals with direction based movement controller, horizontal input will change the rotation angle, and vertical input will move the object forward or backward.*/
public class DirectionMovementController : MovementControllerBase
{
    public void Move(Transform transform, float moveSpeed, float turnSpeed)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed * verticalInput, Space.Self);
        transform.Rotate(Vector3.forward, -Time.deltaTime * turnSpeed * horizontalInput);
    }
}
