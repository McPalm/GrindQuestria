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
        long seed = Random.Range(0, 10000);
        long seed2 = seed + Random.Range(100, 10000);

        for (int x = -size.x; x < size.x; x++)
        {
            for (int y = -size.y; y < size.y; y++)
            {
                var localValue = OpenSimplex2S.Noise2(seed, x * .15f, y *.15f) + OpenSimplex2S.Noise2(seed, x * .05f, y * .05f);
                Tilemap.SetTile(new Vector3Int(x, y, 0), localValue < 0 ? Tiles[0] : Tiles[1]);
            }
        }
    }

}
