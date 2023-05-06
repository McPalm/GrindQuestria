using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetInput : NetworkBehaviour
{
    public GameObject PlayerObject;

    [SyncVar]
    public GameObject MyCharacter;

    IEnumerator Start()
    {
        if (isServer)
        {
            var player = Instantiate(PlayerObject);
            NetworkServer.Spawn(player);
            MyCharacter = player;
            player.GetComponent<Movement>().enabled = true;
        }
        if(isLocalPlayer)
        {
            while (MyCharacter == null)
                yield return null;
            InitLocal();
        }
    }

    /// <summary> Hook up UI and controls. </summary>
    void InitLocal()
    {
        FindObjectOfType<TaskProgressBar>().Target = MyCharacter.GetComponent<Interactor>();
        FindObjectOfType<InventoryUI>().target = MyCharacter.GetComponent<Inventory>();
        FindObjectOfType<CameraFollow>().follow = MyCharacter.transform;
        FindObjectOfType<ClickToMove>().netInput = this;
        FindObjectOfType<BlueprintEditor>(includeInactive: true).NetInput = this;
    }

    [Command]
    public void DoThing(DoThing.ThingToDo thing)
    {
        MyCharacter.GetComponent<DoThing>().DoThingImmediately(thing);
    }
    [Command]
    public void SetBlueprint(Vector3Int position, int tileIndex)
    {
        var tile = TileCollection.Instance.GetTile(tileIndex);
        GridManager.instance.Blueprint.SetTile(position, tile);
    }
}
