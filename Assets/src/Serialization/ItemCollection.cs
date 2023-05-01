using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemCollection : ScriptableObjectCollection<Item>
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
