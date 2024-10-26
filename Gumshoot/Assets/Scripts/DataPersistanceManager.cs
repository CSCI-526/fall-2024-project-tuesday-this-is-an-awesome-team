using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance = null;
    public SaveObject[] saveObjects;
    [SerializeField]  private Dictionary<int, bool> saveStates = new();
    public Vector3 newPos = Vector3.zero;
    public bool hasNewPos = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Instance.saveObjects = FindObjectsOfType<SaveObject>();
            Instance.Load();
            Destroy(gameObject);
            return;
        }

        Instance = this;
        saveObjects = FindObjectsOfType<SaveObject>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        saveStates.Clear();
        foreach (SaveObject save in saveObjects)
        {
            saveStates.Add(save.ID, false);
        }
    }

    private void Load()
    {
        foreach (SaveObject save in saveObjects)
        {
            if (saveStates.TryGetValue(save.ID, out var state))
            {
                if (state == true)
                {
                    save.Load();
                }
            }
        }
    }

    public void Save(Vector3 playerPos)
    {
        foreach (SaveObject save in saveObjects)
        {
            if (save.State == true)
            {
                saveStates[save.ID] = true;
            }
        }

        newPos = playerPos;
    }
}
