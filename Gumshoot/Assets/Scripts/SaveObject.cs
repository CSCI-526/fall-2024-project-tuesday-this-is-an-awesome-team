using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveObject : MonoBehaviour
{
    public int ID;
    public bool State = false;
    public UnityEvent onActivate;

    public void Load()
    {
        State = true;
        onActivate?.Invoke();
    }
}
