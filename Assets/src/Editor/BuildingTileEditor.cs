using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(BuildingTile))]
[CanEditMultipleObjects]
public class BuildingTileEditor : Editor
{
    public BuildingTile Tile => target as BuildingTile;

    public override bool HasPreviewGUI() => true;

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
        if (Tile.sprite != null)
        {
            Type t = RuleTileEditor.GetType("UnityEditor.SpriteUtility");
            if (t != null)
            {
                System.Reflection.MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) });
                if (method != null)
                {
                    object ret = method.Invoke("RenderStaticPreview", new object[] { Tile.sprite, Color.white, width, height });
                    if (ret is Texture2D)
                        return ret as Texture2D;
                }
            }
        }
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }
}
