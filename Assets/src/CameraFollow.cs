using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform follow;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (follow)
            transform.position = Vector3.Lerp(transform.position, follow.position, .15f);
        
    }
}
