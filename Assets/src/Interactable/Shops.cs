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

    public void Interact(GameObject user, Vector3 worldPosition)
    {
        Debug.Log("And nothing Happened!");
    }

    public float TimeToComplete(Vector3 worldPosition) => 1f;
    
    void Awake()
    {
        Instance = this;
    }

    public void OpenShopUI(Vector3Int where)
    {
        var shop = ShopAt(where);
        if(shop != null)
            FindObjectOfType<CraftMenuUI>(true).Open(shop);
    }

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
}
