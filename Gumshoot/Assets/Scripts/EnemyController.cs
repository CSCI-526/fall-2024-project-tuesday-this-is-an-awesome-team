using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected float cooldownDuration;

    public void Init(float timer)
    {
        cooldownDuration = timer;
    }
}
