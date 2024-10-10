using UnityEngine;

public class NewFlyingEnemyController : EnemyControllerBase
{
    protected override void Start()
    {
        base.Start();
        movementStrategy = new FlyingMovementController();
    }
}
