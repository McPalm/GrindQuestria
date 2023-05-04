using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[CreateAssetMenu]
public class TileCollection : ScriptableObjectCollection<TileBase>
{
    static TileCollection instance;
    public static TileCollection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<TileCollection>(typeof(TileCollection).ToString());
            }
            return instance;
        }
    }

    public TileBase GetTile(int index) => index == -1 ? null : List[index];
    public int IndexOf(TileBase tile) => tile == null ? -1 : List.IndexOf(tile);

}
