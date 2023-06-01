using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Containers : NetworkBehaviour, IInteractable
{
    public TileBase Crate;

    public bool ValidTarget => true;

    public float MinimumDistance => 1.25f;
    NetTilemap tilemap;

    Container container;

    Dictionary<Vector3Int, Container> MapContainers;
    Dictionary<GameObject, Container> Inventories;

    static public Containers Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tilemap = GridManager.instance.Walls;
        container = new Container(11);
    }

    public Vector2 GetInteractLocation(Vector3 worldPosition) => tilemap.CenterOf(worldPosition);

    public void Interact(GameObject user, DoThing.ThingToDo info)
    {
        Debug.Log("Open");
        var ui = FindObjectOfType<StorageUI>();
        IContainer inventory = user.GetComponent<Inventory>();
        IContainer container = ContainerAt(info.where);
        System.Action<ItemBundle> moveLeft = delegate (ItemBundle bundle) {
            Debug.Log(bundle.item.displayName);
        };
        System.Action<ItemBundle> moveRight = delegate (ItemBundle bundle) {
            Debug.Log(bundle.item.displayName);
        };
        ui.Open(inventory, container, moveRight, moveLeft);
    }

    public float TimeToComplete(DoThing.ThingToDo info) => 0f;


    public (bool canUse, string failmessage) ValidateUse(GameObject user, DoThing.ThingToDo info)
    {
        return (true, null);
    }

    public bool HasContainerAt(Vector3 position)
    {
        return tilemap.GetTile(position) == Crate;
    }

    Container ContainerAt(Vector3 position) => ContainerAt(GridManager.instance.Walls.WorldToCell(position));
    Container ContainerAt(Vector3Int position)
    {
        if (MapContainers.ContainsKey(position) == false)
            MapContainers.Add(position, new Container(11));
        return MapContainers[position];
    }

    Container InventoryOf(GameObject character)
    {
        if (Inventories.ContainsKey(character) == false)
            Inventories.Add(character, new Container(88));
        return Inventories[character];
    }

    public void MoveItem(ItemBundle itemBundle, Container from, Container to)
    {
        from.RemoveItems(itemBundle);
        to.AddItems(itemBundle);
    }
}
