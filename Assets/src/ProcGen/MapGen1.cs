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
    public GameObject Rock;


    // Utility
    int X(int value) => value % size;
    int Y(int value) => value / size;
    Vector3Int Cell(int value) => new Vector3Int(X(value) - size / 2, Y(value) - size / 2, 0);
    float SquareDistanceFromCenter(int value) => Mathf.Abs((X(value) - size / 2) * (Y(value) - size / 2));
    HashSet<Vector3Int> UsedSpots;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        yield return Generate();
    }

    float[] heightMap;

    [Server]
    IEnumerator Generate()
    {
        UsedSpots = new HashSet<Vector3Int>();
        yield return GenerateHeight();
        yield return PaintGrassBasedOnHeight();
        yield return PlaceTrees();
        yield return PlaceRocks();
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
            if (IsAvailable(Cell(i)))
            {
                if (SquareDistanceFromCenter(i) > 100f && heightMap[i] - Random.value < -.7f && Random.value < .3f)
                {
                    // place tree
                    var o = Instantiate(Tree, Cell(i) + new Vector3(Random.value, Random.value), Quaternion.identity);
                    NetworkServer.Spawn(o);
                    ClaimRadius(Cell(i));
                }
            }
        }
        yield return Process(A);
    }

    IEnumerator PlaceRocks()
    {
        void A(int i)
        {
            if(IsAvailable(Cell(i)))
            {
                if(SquareDistanceFromCenter(i) > 100f && (heightMap[i] + Random.value) > 0f && Random.value < .01f)
                {
                    var o = Instantiate(Rock, Cell(i) + new Vector3(Random.value, Random.value), Quaternion.identity);
                    NetworkServer.Spawn(o);
                    ClaimSpot(Cell(i));
                }
            }
        }
        yield return Process(A);
    }

    bool IsAvailable(Vector3Int spot) => !UsedSpots.Contains(spot);
    void ClaimSpot(Vector3Int spot) => UsedSpots.Add(spot);
    void ClaimRadius(Vector3Int spot)
    {
        ClaimSpot(spot);
        ClaimSpot(spot + Vector3Int.up);
        ClaimSpot(spot + Vector3Int.right);
        ClaimSpot(spot + Vector3Int.down);
        ClaimSpot(spot + Vector3Int.left);
    }

}
