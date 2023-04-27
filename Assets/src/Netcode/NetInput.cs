using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetInput : NetworkBehaviour
{
    public GameObject PlayerObject;

    [SyncVar]
    GameObject Character;

    IEnumerator Start()
    {
        if (isServer)
        {
            var player = Instantiate(PlayerObject);
            var netID = player.GetComponent<NetworkIdentity>();
            NetworkServer.Spawn(player);
            Character = player;
            player.GetComponent<Movement>().enabled = true;
        }
        if(isLocalPlayer)
        {
            while (Character == null)
                yield return null;
            InitLocal();
        }
    }

    /// <summary> Hook up UI and controls. </summary>
    void InitLocal()
    {
        FindObjectOfType<TaskProgressBar>().Target = Character.GetComponent<Interactor>();
        FindObjectOfType<InventoryUI>().target = Character.GetComponent<Inventory>();
        FindObjectOfType<CameraFollow>().follow = Character.transform;
        GetComponent<ClickToMove>().enabled = true;
    }

    [Command]
    public void DoThing(DoThing.ThingToDo thing)
    {
        Character.GetComponent<DoThing>().DoThingImmediately(thing);
    }

}
