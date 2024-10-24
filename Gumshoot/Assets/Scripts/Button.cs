using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent OnPress;

    private bool isPressed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Block"))
        {
            Press();
        }
    }

    public void Press()
    {
        isPressed = true;
        GetComponent<SpriteRenderer>().color = Color.green;
        OnPress?.Invoke();
        SaveObject save = GetComponent<SaveObject>();
        if (save)
        {
            save.State = true;
        }
    }
}
