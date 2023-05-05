using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu]
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

    public int IndexOf(Item item) => item == null ? -1 : List.IndexOf(item);
    public Item GetItem(int index) => index == -1 ? null : List[index];
}
