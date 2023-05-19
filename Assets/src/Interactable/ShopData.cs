using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class ShopData : ScriptableObject
{
    public float craftTimeMultiplier = 1f;
    public string displayName;
    public Recepie[] Recepies;
    public TileBase Tile;
}
