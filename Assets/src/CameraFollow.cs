using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform follow;

    // Update is called once per frame
    void Update()
    {
        if(follow)
            transform.position = follow.position;
    }
}
