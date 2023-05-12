using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotkey1to9 : MonoBehaviour
{
    public Button[] buttons;

    KeyCode[] keys;

    private void Awake()
    {
        keys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
                buttons[i].onClick.Invoke();
        }
    }
}
