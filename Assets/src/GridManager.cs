using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    static public GridManager instance;
    public NetTilemap Ground;
    public NetTilemap Walls;
    public NetTilemap Blueprint;

    void Awake()
    {
        instance = this;
    }

    static public NetTilemap GetLayer(TileLayer tileLayer)
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
