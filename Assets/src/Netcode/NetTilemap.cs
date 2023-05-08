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

    Tilemap Tilemap { get
        {
            /*
            if (tilemap == null)
                tilemap = GetComponent<Tilemap>();
            */
            return tilemap;
        }
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void SetTile(Vector3 worldPosition, TileBase tile) => SetTile(Tilemap.WorldToCell(worldPosition), tile);
    public void SetTile(Vector3Int position, TileBase tile)
    {
#if UNITY_SERVER
        tilemap.SetTile(change.position, change.Tile);
#endif
        ChangeTile(SerializedSyncTile.Create(position, tile));
    }

    [Command(requiresAuthority =false)]
    void RequestTiles(Vector3Int center, NetworkConnectionToClient sender = null) => SendTileRequest(center, sender);

    [Server]
    public void SendTileRequest(Vector3Int center, NetworkConnectionToClient destination)
    {
        var area = GetBoundsAround(center);
        var tiles = Tilemap.GetTilesBlock(area);
        int[] tileIndexes = new int[tiles.Length];
        for (int i = 0; i < tileIndexes.Length; i++)
        {
            tileIndexes[i] = TileCollection.Instance.IndexOf(tiles[i]);
        }
        ChangeTiles(destination, center, tileIndexes);
    }

    [TargetRpc]
    void ChangeTiles(NetworkConnection connectionToClient, Vector3Int center, int[] tiles)
    {
        var area = GetBoundsAround(center);
        int i = 0;
        foreach(var position in area.allPositionsWithin)
        {
            Tilemap.SetTile(position, TileCollection.Instance.GetTile(tiles[i++]));
        }
    }

    [ClientRpc]
    void ChangeTile(SerializedSyncTile change)
    {
        Tilemap.SetTile(change.position, change.Tile);
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

    public Vector3Int WorldToCell(Vector3 worldPosition) => Tilemap.WorldToCell(worldPosition);
    public Vector3 CellToWorld(Vector3Int cellPosition) => Tilemap.CellToWorld(cellPosition);
    public TileBase GetTile(Vector3Int cellPosition) => Tilemap.GetTile(cellPosition);
    public TileBase GetTile(Vector3 worldPosition) => GetTile(WorldToCell(worldPosition));

    BoundsInt GetBoundsAround(Vector3Int center) => new BoundsInt(center - new Vector3Int(20, 20, 0), new Vector3Int(40, 40, 1));
}
;