using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionModeToggle : MonoBehaviour
{
    public GameObject[] BluePrintModeObjects;
    public MonoBehaviour[] BluePrintModeComponents;
    public GameObject[] BuildingModeObjects;
    public MonoBehaviour[] BuildingModeComponents;

    public InteractionModes Mode { get; private set; }
    
    void Start()
    {
        SetMode(InteractionModes.build);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SetMode(InteractionModes.build);
        if (Input.GetKeyDown(KeyCode.F2))
            SetMode(InteractionModes.blueprint);
    }

    public void SetMode(InteractionModes mode)
    {
        DisableAll();
        Mode = mode;
        switch(mode)
        {
            case InteractionModes.blueprint:
                foreach (var o in BluePrintModeObjects)
                    o.SetActive(true);
                foreach (var b in BluePrintModeComponents)
                    b.enabled = false;
                break;
            case InteractionModes.build:
                foreach (var o in BuildingModeObjects)
                    o.SetActive(true);
                foreach (var b in BuildingModeComponents)
                    b.enabled = true;
                break;
        }
    }

    void DisableAll()
    {
        foreach (var o in BluePrintModeObjects)
            o.SetActive(false);
        foreach (var b in BluePrintModeComponents)
            b.enabled = false;
        foreach (var o in BuildingModeObjects)
            o.SetActive(false);
        foreach (var b in BuildingModeComponents)
            b.enabled = false;
    }

    public enum InteractionModes
    {
        build,
        blueprint
    }
}
