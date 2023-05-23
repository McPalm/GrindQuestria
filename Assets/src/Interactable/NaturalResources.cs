using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalResources : NetworkBehaviour, IInteractable
{
    static NaturalResources instance;
    static public NaturalResources Instance => instance;

    public NetTilemap Walls;

    void Awake()
    {
        instance = this;
    }

    public bool ValidTarget => true;

    public float MinimumDistance => 1f;

    public Vector2 GetInteractLocation(Vector3 worldPosition)
    {
        return Walls.CellToWorld(Walls.WorldToCell(worldPosition));
    }

    public void Interact(GameObject user, DoThing.ThingToDo info)
    {
        var inventory = user.GetComponent<Inventory>();
        var tile = GetTile(info.where);
        if(tile != null)
        {
            inventory.AddBundles(tile.Drops);
        }
        Walls.SetTile(info.where, tile.breaksInto);
    }

    public float TimeToComplete(DoThing.ThingToDo info)
    {
        var tile = GetTile(info.where);
        if (tile != null)
            return tile.breakTime;
        return 1f;
    }

    public (bool canUse, string failmessage) ValidateUse(GameObject user, DoThing.ThingToDo info)
    {
        var tile = GetTile(info.where);
        if (tile == null)
            return (false, "Nothing There");
        return (true, null);
    }

    BreakableTile GetTile(Vector3 position)
    {
        var tile = Walls.GetTile(position);
        if (tile != null && tile is BreakableTile)
            return tile as BreakableTile;
        return null;
    }

    public bool HasResourcesHere(Vector3 position) => GetTile(position) != null;
}
