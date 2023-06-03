using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetInput : NetworkBehaviour
{
    [SyncVar]
    public GameObject MyCharacter;
    ShopOpener shopOpener;

    IEnumerator Start()
    {
        if (isServer)
        {
            var player = CharacterRegistry.Instance.GetCharacter();
            MyCharacter = player;
            player.GetComponent<DoThing>().enabled = true;
            var identity = GetComponent<NetworkIdentity>();
            player.GetComponent<NetCharacterID>().Connection = identity.connectionToClient;
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
        shopOpener = MyCharacter.AddComponent<ShopOpener>();
        shopOpener.StartCraft += CraftItemAtShop;
        Containers.Instance.InventoryOf(MyCharacter).OnAdd += ItemGetPopupUI.Instance.ShowItemGet;
    }

    private void OnDestroy()
    {
        if(isServer)
            CharacterRegistry.Instance.YieldCharacter(MyCharacter);
    }


    public void OpenShop(Vector3 location)
    {
        shopOpener.openShop = true;
        shopOpener.where = location;
    }

    public void DoThing(DoThing.ThingToDo thing)
    {
        shopOpener.openShop = false;
        CmdDoThing(thing);
    }
    [Command]
    public void CmdDoThing(DoThing.ThingToDo thing)
    {
        MyCharacter.GetComponent<DoThing>().DoThingImmediately(thing);
    }
    [Command]
    public void SetBlueprint(Vector3Int position, int tileIndex)
    {
        var tile = TileCollection.Instance.GetTile(tileIndex);
        GridManager.instance.Blueprint.SetTile(position, tile);
    }

    private class ShopOpener : MonoBehaviour
    {
        public bool openShop;
        public Vector2 where;
        public event System.Action<Vector3Int, int> StartCraft;

        void Update()
        {
            if (openShop)
            {
                var sqrDistance = ((Vector2)transform.position - where).sqrMagnitude;
                if (sqrDistance < 1f)
                {
                    OpenShop();
                }
            }
        }

        void OpenShop()
        {
            var gridlocation = GridManager.instance.Walls.WorldToCell(where);
            Shops.Instance.OpenShopUI(gridlocation, (int i) => StartCraft?.Invoke(gridlocation, i));
            openShop = false;
        }
    }

    private void CraftItemAtShop(Vector3Int shopLocation, int craftIndex)
    {
        CmdDoThing(new DoThing.ThingToDo()
        {
            what = global::DoThing.Things.interact,
            who = Shops.Instance.gameObject,
            where = GridManager.instance.Walls.CellToWorld(shopLocation),
            number = craftIndex,
        });
    }
}
