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
        int low = 0;
        int high = 0;
        for (int x = -size.x; x < size.x; x++)
        {
            for (int y = -size.y; y < size.y; y++)
            {
                var localValue = .8f * Mathf.PerlinNoise(x * .2f + rx + y * .03f, y * .2f + ry + x * .03f) + Random.value * .25f;
                int goodValue = Mathf.Clamp(Mathf.FloorToInt(localValue * Tiles.Length), 0, Tiles.Length - 1);
                if (goodValue == 0)
                    low++;
                if (goodValue == 1)
                    high++;
                Tilemap.SetTile(new Vector3Int(x, y, 0),  Tiles[goodValue]);
            }
        }
        Debug.Log($"Bright: {low}, Dark: {high}");
    }

}
