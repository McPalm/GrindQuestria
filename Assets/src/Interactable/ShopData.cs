using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class ShopData : ScriptableObject
{
    public string IngameName;
    public Recepie[] Recepies;
    public TileBase Tile;
}
