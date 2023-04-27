using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTask : MonoBehaviour
{
    public void CreateTask(Vector3 at)
    {
        at = Vector3Int.FloorToInt(at) + new Vector3(.5f, .5f, 0f);
        transform.position = at;
        GetComponent<SpriteRenderer>().sprite = building.tile.sprite;
    }

    public Building building;

    bool completed = false;

    public bool ValidTarget => !completed;
    public float MinimumDistance => 1f;
    public float TimeToComplete => 1f;

    public Vector2 InteractLocation => transform.position;

    public void Interact(GameObject user)
    {
        completed = true;
        var tilemap = GridManager.GetLayer(building.tileLayer);
        Vector3Int place = Vector3Int.FloorToInt(transform.position);
        tilemap.SetTile(Vector3Int.FloorToInt(transform.position), building.tile);
        gameObject.SetActive(false);
        throw new System.Exception("deprechiated function");
    }
}
