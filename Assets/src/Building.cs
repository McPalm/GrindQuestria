using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu, System.Serializable]
public class Building : ScriptableObject
{
    public Item[] materials;
    public int[] materialsQTY;
    public Tile tile;
    public TileLayer tileLayer;
}
