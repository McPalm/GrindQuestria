using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class ShopData : ScriptableObject, ICraftMenuClient
{
    public float craftTimeMultiplier = 1f;
    public string displayName;
    public Recepie[] Recepies;
    public TileBase Tile;

    public string CraftMenuTitle => displayName;
    public IEnumerable<ICraftMenuItem> Items => Recepies;
}
