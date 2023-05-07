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
        if(isClientOnly)
        {
            RequestTiles(Vector3Int.zero);
        }
    }

    public void SetTile(Vector3 worldPosition, TileBase tile) => SetTile(tilemap.WorldToCell(worldPosition), tile);
    public void SetTile(Vector3Int position, TileBase tile)
    {
#if UNITY_SERVER
        tilemap.SetTile(change.position, change.Tile);
#endif
        ChangeTile(SerializedSyncTile.Create(position, tile));
    }

    [Command(requiresAuthority =false)]
    void RequestTiles(Vector3Int center, NetworkConnectionToClient sender = null)
    {
        var area = new BoundsInt(center - new Vector3Int(10, 10, 0), new Vector3Int(20, 20, 1));
        var tiles = tilemap.GetTilesBlock(area);
        int[] tileIndexes = new int[tiles.Length];
        for (int i = 0; i < tileIndexes.Length; i++)
        {
            tileIndexes[i] = TileCollection.Instance.IndexOf(tiles[i]);
        }
        ChangeTiles(sender, center, tileIndexes);
    }

    [TargetRpc]
    void ChangeTiles(NetworkConnection connectionToClient, Vector3Int center, int[] tiles)
    {
        var area = new BoundsInt(center - new Vector3Int(10, 10, 0), new Vector3Int(20, 20, 1));
        int i = 0;
        foreach(var position in area.allPositionsWithin)
        {
            tilemap.SetTile(position, TileCollection.Instance.GetTile(tiles[i++]));
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
    public TileBase GetTile(Vector3 worldPosition) => GetTile(WorldToCell(worldPosition));
}
;