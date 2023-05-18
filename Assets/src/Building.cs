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
}
