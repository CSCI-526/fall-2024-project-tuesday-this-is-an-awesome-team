using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent OnPress;
    public UnityEvent OnRelease; // 添加一个事件用于释放按钮时触发

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

    void OnTriggerExit2D(Collider2D other)
    {
        if (isPressed && other.CompareTag("Block"))
        {
            Release();
        }
    }

    public void Press()
    {
        isPressed = true;

        if (buttonOff != null) buttonOff.SetActive(false);
        if (buttonOn != null) buttonOn.SetActive(true);

        OnPress?.Invoke();
        SaveObject save = GetComponent<SaveObject>();
        if (save)
        {
            save.State = true;
        }
    }

    public void Release()
    {
        isPressed = false;

        if (buttonOff != null) buttonOff.SetActive(true);
        if (buttonOn != null) buttonOn.SetActive(false);

        OnRelease?.Invoke(); 
        SaveObject save = GetComponent<SaveObject>();
        if (save)
        {
            save.State = false;
        }
    }

    public bool IsPressed
    {
        get { return isPressed; }
    }
}

