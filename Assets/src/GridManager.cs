using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    static public GridManager instance;
    public Tilemap Ground;
    public Tilemap Walls;
    public Tilemap Blueprint;

    void Awake()
    {
        instance = this;
    }

    static public Tilemap GetLayer(TileLayer tileLayer)
    {
        switch (tileLayer)
            {
            case TileLayer.ground: return instance.Ground;
            case TileLayer.wall: return instance.Walls;
            case TileLayer.blueprint: return instance.Blueprint;
        }
        throw new System.Exception($"unsupported layer {tileLayer}");
    }
}
