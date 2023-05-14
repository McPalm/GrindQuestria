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
        tileData.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        tileData.flags = TileFlags.LockTransform;
        if (tilemap.cellBounds.zMax > 1) // hack to smuggle grid ID out to us.
        {
            tileData.gameObject = null;
            tileData.colliderType = ColliderType.Grid;
        }
        if (sprites != null && sprites.Length > 0)
        {
            tileData.sprite = sprites[Mathf.Abs(position.x + position.y)%sprites.Length];
        }
        //var m = tileData.transform;
        // m.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);
    }

}
