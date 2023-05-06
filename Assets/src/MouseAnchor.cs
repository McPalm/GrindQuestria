using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAnchor : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
