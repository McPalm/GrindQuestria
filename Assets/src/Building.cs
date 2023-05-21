using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu, System.Serializable]
public class Building : ScriptableObject
{
    public ItemBundle[] materials;
    public TileBase tile;
    public TileLayer tileLayer;
    public Category category;

    public enum Category
    {
        wall = 0,
        floor = 1,
        door = 2,
        shop = 3,
        furniture = 4,
    }
}
