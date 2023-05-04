using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tilemap;

[RequireComponent(typeof(Tilemap))]
public class NetTilemap : NetworkBehaviour
{
    Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void SetTile(Vector3Int position, TileBase tile)
    {
        ChangeTile(SerializedSyncTile.Create(position, tile));
    }

    [ClientRpc]
    void ChangeTiles(SerializedSyncTile[] changes)
    {
        foreach(var change in changes)
        {
            tilemap.SetTile(change.position, change.Tile);
        }
    }

    [ClientRpc]
    void ChangeTile(SerializedSyncTile change)
    {
        tilemap.SetTile(change.position, change.Tile);
    }

    [System.Serializable]
    public struct SerializedSyncTile
    {
        public Vector3Int position;
        public int tileIndex;

        static public SerializedSyncTile Create(Vector3Int position, TileBase tile)
        {
            return new SerializedSyncTile()
            {
                position = position,
                tileIndex = TileCollection.Instance.IndexOf(tile),
            };
        }

        public TileBase Tile => TileCollection.Instance.GetTile(tileIndex);
    }

    public Vector3Int WorldToCell(Vector3 worldPosition) => tilemap.WorldToCell(worldPosition);
    public Vector3 CellToWorld(Vector3Int cellPosition) => tilemap.CellToWorld(cellPosition);
    public TileBase GetTile(Vector3Int cellPosition) => tilemap.GetTile(cellPosition);
}
;