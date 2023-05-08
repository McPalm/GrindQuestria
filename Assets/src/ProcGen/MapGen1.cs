using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen1 : MonoBehaviour
{
    public int size = 200;
    public TileBase Grass1;
    public TileBase Grass2;
    public GameObject Tree;


    // Utility
    int X(int value) => value % size;
    int Y(int value) => value / size;
    Vector3Int Cell(int value) => new Vector3Int(X(value) - size / 2, Y(value) - size / 2, 0);
    float SquareDistanceFromCenter(int value) => Mathf.Abs((X(value) - size / 2) * (Y(value) - size / 2));


    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        yield return Generate();
    }

    float[] heightMap;

    [Server]
    IEnumerator Generate()
    {
        yield return GenerateHeight();
        yield return null;
        yield return PaintGrassBasedOnHeight();
        yield return null;
        yield return PlaceTrees();
    }

    IEnumerator Process(System.Action<int> action)
    {
        var sec = Time.realtimeSinceStartupAsDouble + 0.05;
        for (int i = 0; i < heightMap.Length; i++)
        {
            action(i);
            if (Time.realtimeSinceStartupAsDouble > sec)
            {
                yield return null;
                sec = Time.realtimeSinceStartupAsDouble + 0.05;
            }
        }
    }

    IEnumerator GenerateHeight()
    {
        heightMap = new float[size * size];
        long seed = Random.Range(0, 10000);
        long seed2 = seed + Random.Range(0, 10000);
        void A(int i) => heightMap[i] = OpenSimplex2S.Noise2(seed, X(i) * .15f, Y(i) * .15f) + OpenSimplex2S.Noise2(seed2, X(i) * .03f, Y(i) * .03f);
        yield return Process(A);
    }


    IEnumerator PaintGrassBasedOnHeight()
    {
        var tilemap = GridManager.instance.Ground.GetComponent<Tilemap>();
        void A(int i) => tilemap.SetTile(Cell(i), heightMap[i] < 0 ? Grass1 : Grass2);
        yield return Process(A);
    }

    IEnumerator PlaceTrees()
    {
        void A(int i)
        {
            if (SquareDistanceFromCenter(i) > 100f && heightMap[i] - Random.value < -.7f && Random.value < .3f)
            {
                // place tree
                var o = Instantiate(Tree, Cell(i) + new Vector3(Random.value, Random.value), Quaternion.identity);
                NetworkServer.Spawn(o);
            }
        }
        yield return Process(A);
    }
}
