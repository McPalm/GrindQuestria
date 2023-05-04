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
            GridManager.GetLayer(TileLayer.wall).SetTile(cellPos, null);
            tilemap.SetTile(cellPos, null);
            return;
        }
        // check if we have the materials
        var inventory = user.GetComponent<Inventory>();
        if (inventory.HasItem(building.materials[0]) == false)
            return;
        inventory.RemoveItem(building.materials[0], building.materialsQTY[0]);
        // remove from blueprint and add to tilemap
        GridManager.GetLayer(building.tileLayer).SetTile(cellPos, blueprintTile);
        tilemap.SetTile(cellPos, null);
    }
}
