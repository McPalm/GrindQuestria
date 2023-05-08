using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetInput : NetworkBehaviour
{
    [SyncVar]
    public GameObject MyCharacter;

    IEnumerator Start()
    {
        if (isServer)
        {
            var player = CharacterRegistry.Instance.GetCharacter();
            MyCharacter = player;
            player.GetComponent<DoThing>().enabled = true;
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

    private void OnDestroy()
    {
        if(isServer)
            CharacterRegistry.Instance.YieldCharacter(MyCharacter);
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
