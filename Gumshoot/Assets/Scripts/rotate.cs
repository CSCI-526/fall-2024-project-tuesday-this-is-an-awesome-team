using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 50.0f;
    private bool isSpinning = false;

    public void StartFan()
    {
        isSpinning = true;
    }

    public void StopFan()
    {
        isSpinning = false;
    }

    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            Transform canvasTransform = transform.Find("Canvas");
            if (canvasTransform != null)
            {
                Image imageComponent = canvasTransform.GetComponentInChildren<Image>();
                if (imageComponent != null)
                {
                    imageComponent.color = Color.green;
                }
            }
        }
    }
}