using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[CreateAssetMenu]
public class TileCollection : ScriptableObjectCollection<TileBase>
{
    static ItemCollection instance;
    public static ItemCollection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<ItemCollection>(typeof(ItemCollection).ToString());
            }
            return instance;
        }
    }
}
