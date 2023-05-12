using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour, IInteractable
{
    public NetTilemap blueprintTilemap;

    public bool ValidTarget => true;
    public float MinimumDistance => 1f;
    public float wallBuildTime = 7f;
    public float floorBuildtime = 3f;
    public float TimeToComplete(Vector3 worldPosition) => HasWall(worldPosition) ? wallBuildTime : floorBuildtime;


    public Vector2 GetInteractLocation(Vector3 worldPosition)
    {
        return blueprintTilemap.CellToWorld(blueprintTilemap.WorldToCell(worldPosition)) + new Vector3(.5f, .5f, 0f);
    }

    public void Interact(GameObject user, Vector3 worldPosition)
    {
        // check if task is still available
        var cellPos = blueprintTilemap.WorldToCell(worldPosition);
        var blueprintTile = blueprintTilemap.GetTile(cellPos);
        if (blueprintTile == null)
            return;
        var building = ((BuildingTile)blueprintTile).building;
        // check if we are bulldozing
        if(building == null)
        {
            DemolishAndRefund(user, cellPos);
            blueprintTilemap.SetTile(cellPos, null);
            return;
        }
        // check if we have the materials
        var inventory = user.GetComponent<Inventory>();
        if (inventory.HasItem(building.materials[0]) == false)
            return;
        inventory.RemoveItem(building.materials[0], building.materialsQTY[0]);
        // remove old wall if any
        if (GridManager.GetLayer(building.tileLayer).GetTile(cellPos) != null)
            DemolishAndRefund(user, cellPos);
        // remove from blueprint and add to tilemap        
        GridManager.GetLayer(building.tileLayer).SetTile(cellPos, blueprintTile);
        blueprintTilemap.SetTile(cellPos, null);
    }

    public void DemolishAndRefund(GameObject user, Vector3Int position)
    {
        var tile = GridManager.instance.Walls.GetTile(position);
        if(tile != null && tile is BuildingTile)
        {
            var bTile = tile as BuildingTile;
            var inventory = user.GetComponent<Inventory>();
            for(int i = 0; i < bTile.building.materials.Length; i++)
            {
                inventory.AddItem(bTile.building.materials[i], bTile.building.materialsQTY[i]);
            }
        }
        GridManager.instance.Walls.SetTile(position, null);
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
            return b.building.tileLayer == TileLayer.wall;
        }
        return true;
    }
}
