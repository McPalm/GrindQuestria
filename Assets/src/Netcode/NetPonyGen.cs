using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetPonyGen : NetworkBehaviour
{
    [SyncVar]
    PonyGen.SerializedApperance apperance;
    
    void Start()
    {
        var pony = GetComponentInChildren<PonyGen>();
        if(isServer)
        {
            pony.Generate();
            apperance = pony.GetApperance();
        }
        else
        {
            pony.generate = false;
            pony.SetApperance(apperance);
        }
    }

}
