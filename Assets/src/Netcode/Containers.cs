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

    static public Containers Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tilemap = GridManager.instance.Walls;
    }

    public Vector2 GetInteractLocation(Vector3 worldPosition) => tilemap.CenterOf(worldPosition);

    public void Interact(GameObject user, DoThing.ThingToDo info)
    {
        Debug.Log("Open");
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
}
