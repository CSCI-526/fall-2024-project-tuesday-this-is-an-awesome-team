using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent OnPress;

    public GameObject buttonOn;
    public GameObject buttonOff;

    private bool isPressed = false;

    void Start()
    {
        if (buttonOff != null) buttonOff.SetActive(true);
        if (buttonOn != null) buttonOn.SetActive(false);
    }

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

        if (buttonOff != null) buttonOff.SetActive(false);
        if (buttonOn != null) buttonOn.SetActive(true);
        //GetComponent<SpriteRenderer>().color = Color.green;

        OnPress?.Invoke();
        SaveObject save = GetComponent<SaveObject>();
        if (save)
        {
            save.State = true;
        }
    }

    public bool IsPressed
    {
        get { return isPressed; }
    }
}
