using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePerlinNoise : MonoBehaviour
{
    public Tilemap Tilemap;
    public TileBase[] Tiles;

    public Vector2Int size;

    [Mirror.Server]
    void Start()
    {
        float rx = Random.value * 10000f;
        float ry = Random.value * 10000f;
        for (int x = -size.x; x < size.x; x++)
        {
            for (int y = -size.y; y < size.y; y++)
            {
                var localValue = Mathf.PerlinNoise(x * .2f + rx, y * .2f + ry);
                int goodValue = Mathf.Clamp(Mathf.FloorToInt(localValue * Tiles.Length), 0, Tiles.Length - 1);
                Debug.Log(goodValue);
                Tilemap.SetTile(new Vector3Int(x, y, 0),  Tiles[goodValue]);
            }
        }
    }

}
