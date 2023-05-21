using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu, System.Serializable]
public class Building : ScriptableObject, ICraftMenuItem
{
    public string displayName;
    public ItemBundle[] materials;
    public BuildingTile tile;
    public TileLayer tileLayer;
    public Category category;

    public string ProductName => displayName;
    public Sprite ProductSprite => tile.sprite;
    public IEnumerable<ItemBundle> Materials => materials;

    public enum Category
    {
        wall = 0,
        floor = 1,
        door = 2,
        shop = 3,
        furniture = 4,
    }
}
