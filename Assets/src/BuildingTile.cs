using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class BuildingTile : Tile
{
    public Sprite[] sprites;

    public Building building;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        if(sprites.Length > 0)
        {
            tileData.sprite = sprites[(999999+position.x + position.y)%sprites.Length];
        }
        var m = tileData.transform;
        m.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);
        tileData.transform = m;

    }
}
