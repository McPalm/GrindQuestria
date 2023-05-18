using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Blueprint : MonoBehaviour, IInteractable
{
    public NetTilemap blueprintTilemap;

    public bool ValidTarget => true;
    public float MinimumDistance => 1f;
    public float wallBuildTime = 7f;
    public float floorBuildtime = 3f;
    public float TimeToComplete(DoThing.ThingToDo info) => HasWall(info.where) ? wallBuildTime : floorBuildtime;

    [SerializeField]
    TileBase SmuggledTile;

    void Start()
    {
        GetComponent<Tilemap>().SetTile(new Vector3Int(99, 99, 99), SmuggledTile);
    }

    public Vector2 GetInteractLocation(Vector3 worldPosition)
    {
        return blueprintTilemap.CellToWorld(blueprintTilemap.WorldToCell(worldPosition)) + new Vector3(.5f, .5f, 0f);
    }

    public void Interact(GameObject user, DoThing.ThingToDo info)
    {
        // check if task is still available
        var cellPos = blueprintTilemap.WorldToCell(info.where);
        var blueprintTile = blueprintTilemap.GetTile(cellPos);
        if (blueprintTile == null)
            return;
        var building = ((BuildingTile)blueprintTile).building;
        // check if we are bulldozing
        if(building == null)
        {
            DemolishAndRefund(user, cellPos, TileLayer.wall);
            blueprintTilemap.SetTile(cellPos, null);
            return;
        }
        // check if we have the materials
        var inventory = user.GetComponent<Inventory>();
        if (inventory.HasBundles(building.materials))
            inventory.RemoveBundles(building.materials);
        else
            return;
        // remove old wall if any
        if (GridManager.GetLayer(TileLayer.wall).GetTile(cellPos) != null)
            DemolishAndRefund(user, cellPos, TileLayer.wall);
        // remove old floor if any and were working on floors.
        if (building.tileLayer == TileLayer.ground && GridManager.GetLayer(building.tileLayer).GetTile(cellPos) != null)
            DemolishAndRefund(user, cellPos, building.tileLayer);
        // remove from blueprint and add to tilemap        
        GridManager.GetLayer(building.tileLayer).SetTile(cellPos, blueprintTile);
        blueprintTilemap.SetTile(cellPos, null);
    }

    public void DemolishAndRefund(GameObject user, Vector3Int position, TileLayer tileLayer)
    {
        var tilemap = GridManager.GetLayer(tileLayer);
        var tile = tilemap.GetTile(position);
        if(tile != null && tile is BuildingTile)
        {
            var bTile = tile as BuildingTile;
            var inventory = user.GetComponent<Inventory>();
            for(int i = 0; i < bTile.building.materials.Length; i++)
            {
                inventory.AddBundles(bTile.building.materials);
            }
        }
        tilemap.SetTile(position, null);
    }

    public bool HasWall(Vector3 worldPosition, NetTilemap tilemap = null) => HasWall(blueprintTilemap.WorldToCell(worldPosition), tilemap);
    public bool HasWall(Vector3Int cellPosition, NetTilemap tilemap = null)
    {
        if (tilemap == null)
            tilemap = blueprintTilemap;
        var tile = tilemap.GetTile(cellPosition);
        if (tile == null)
            return false;
        if(tile is BuildingTile)
        {
            var b = tile as BuildingTile;
            if (b.building == null)
                return false;
            return b.building.tileLayer == TileLayer.wall;
        }
        return true;
    }
}
