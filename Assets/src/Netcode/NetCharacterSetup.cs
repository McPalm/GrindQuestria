using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetCharacterSetup : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(isLocalPlayer)
        {
            SetPonyApperance();
        }
    }
    
    void SetPonyApperance()
    {
        CmdPonyApperance(PonyEdit.savedApperance);
    }

    [Command]
    void CmdPonyApperance(PonyGen.SerializedApperance apperance)
    {
        var pony = GetComponent<NetInput>().MyCharacter;
        pony.GetComponent<NetPonyGen>().apperance = apperance;
    }
}
