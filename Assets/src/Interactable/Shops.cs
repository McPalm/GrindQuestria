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

    void OpenShopUI(Vector3Int where)
    {
        Debug.Log("Open Shop!");
    }
}
