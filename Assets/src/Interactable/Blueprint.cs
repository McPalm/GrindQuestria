using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour, IInteractable
{
    public NetTilemap tilemap;

    public bool ValidTarget => true;
    public float MinimumDistance => 1f;
    public float TimeToComplete => 5f;

    public Vector2 GetInteractLocation()
    {
        throw new System.NotImplementedException();
    }

    public Vector2 GetInteractLocation(Vector3 worldPosition)
    {
        return tilemap.CellToWorld(tilemap.WorldToCell(worldPosition)) + new Vector3(.5f, .5f, 0f);
    }

    public void Interact(GameObject user, Vector3 worldPosition)
    {
        // check if task is still available
        var cellPos = tilemap.WorldToCell(worldPosition);
        var blueprintTile = tilemap.GetTile(cellPos);
        if (blueprintTile == null)
            return;
        var building = ((BuildingTile)blueprintTile).building;
        // check if we are bulldozing
        if(building == null)
        {
            DemolishAndRefund(user, cellPos);
            tilemap.SetTile(cellPos, null);
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
        tilemap.SetTile(cellPos, null);
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
}
