using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMover : MonoBehaviour
{
    public float moveDistance = 5f; 
    public float moveSpeed = 2f;    
    private float startPosX;
    private bool movingRight = true;

    void Start()
    {
        startPosX = transform.position.x;
    }

    void Update()
    {
    
        float targetPosX = movingRight ? startPosX + moveDistance : startPosX - moveDistance;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        if (Mathf.Approximately(transform.position.x, targetPosX))
        {
            movingRight = !movingRight;
        }
    }
}
