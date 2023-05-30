using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListUI : MonoBehaviour
{
    public ItemBundleUI[] items;

    public void Open(IContainer container)
    {
        var containerItems = container.GetItems(0, items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            if (i > containerItems.Length)
                items[i].gameObject.SetActive(false);
            else
            {
                items[i].gameObject.SetActive(true);
                items[i].SetItem(containerItems[i]);
            }
        }
    }
}
