using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    public float moveDistance = 15f; 
    public float moveSpeed = 2f;     

    private float startPosY;
    private bool movingUp = true;

    void Start()
    {
        startPosY = transform.position.y;
    }

    void Update()
    {
        float targetPosY = movingUp ? startPosY + moveDistance : startPosY - moveDistance;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPosY, transform.position.z), moveSpeed * Time.deltaTime);

        if (Mathf.Approximately(transform.position.y, targetPosY))
        {
            movingUp = !movingUp;
        }
    }
}
