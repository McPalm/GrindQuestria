using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shops : NetworkBehaviour, IInteractable
{
    static public Shops Instance { get; private set; }

    public NetTilemap WallTilemap;

    public bool ValidTarget => true;
    public float MinimumDistance => 1f;

    public ShopData[] AllShops;

    public Vector2 GetInteractLocation(Vector3 worldPosition)
    {
        return WallTilemap.CellToWorld(WallTilemap.WorldToCell(worldPosition));
    }

    public void Interact(GameObject user, DoThing.ThingToDo info)
    {
        var recepie = RecepieFor(info);
        if (recepie == null)
            return;
        var inventory = user.GetComponent<Inventory>();
        if(inventory.HasBundles(recepie.Materials))
        {
            inventory.RemoveBundles(recepie.Materials);
            inventory.AddBundles(recepie.Product);
        }
        else
        {
            Debug.Log("Missing Items to craft!");
        }
    }

    public float TimeToComplete(DoThing.ThingToDo info) => RecepieFor(info).craftTime * ShopAt(info).craftTimeMultiplier;
    
    void Awake()
    {
        Instance = this;
    }

    public void OpenShopUI(Vector3Int where, System.Action<int> craftAction)
    {
        var shop = ShopAt(where);
        if(shop != null)
            FindObjectOfType<CraftMenuUI>(true).Open(shop, craftAction);
    }

    Recepie RecepieFor(DoThing.ThingToDo info)
    {
        var where = WallTilemap.WorldToCell(info.where);
        var shop = ShopAt(where);
        if (shop == null)
            return null;
        if (info.number < shop.Recepies.Length)
            return shop.Recepies[info.number];
        return null;
    }

    public bool HasShopHere(Vector3 position) => ShopAt(position);
    ShopData ShopAt(Vector3 position) => ShopAt(WallTilemap.WorldToCell(position));
    ShopData ShopAt(DoThing.ThingToDo info) => ShopAt(WallTilemap.WorldToCell(info.where));
    ShopData ShopAt(Vector3Int position)
    {
        var tile = WallTilemap.GetTile(position);
        if (tile == null)
            return null;
        foreach (var shop in AllShops)
            if (shop.Tile == tile)
                return shop;
        return null;
    }

    public (bool canUse, string failmessage) ValidateUse(GameObject user, DoThing.ThingToDo info)
    {
        var inventory = user.GetComponent<Inventory>();
        var shop = ShopAt(info);
        var recepie = shop.Recepies[info.number];
        if (false == inventory.HasBundles(recepie.Materials))
            return (false, "Missing Materials");
        // TODO: Check if already in use
        return (true, null);
    }

}
