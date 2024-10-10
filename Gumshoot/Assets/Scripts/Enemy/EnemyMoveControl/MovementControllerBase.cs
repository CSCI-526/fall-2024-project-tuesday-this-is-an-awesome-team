using UnityEngine;
/*This is the interface for basic movement contorls*/
public interface MovementControllerBase
{
    void Move(Transform transform, float moveSpeed, float turnSpeed);
}
