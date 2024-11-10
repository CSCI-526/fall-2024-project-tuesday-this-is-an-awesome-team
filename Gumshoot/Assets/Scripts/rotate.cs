using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 50.0f;
    private bool isSpinning = false;
    private GameObject centerAxis;

    void Start()
    {
        CreateCenterAxis();
    }

    private void CreateCenterAxis()
    {
        centerAxis = new GameObject("CenterAxis");
        centerAxis.transform.parent = transform;
        centerAxis.transform.localPosition = Vector3.zero;

        SpriteRenderer sr = centerAxis.AddComponent<SpriteRenderer>();
        sr.sprite = CreateBlackSquareSprite();
        sr.sortingOrder = 1;

        centerAxis.transform.localScale = new Vector3(1f, 1f, 1f); 
    }

    private Sprite CreateBlackSquareSprite()
    {
        Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, Color.black);
            }
        }
        texture.Apply();

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }

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
        }
    }
}





