using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListUI : MonoBehaviour
{
    public ItemBundleUI[] items;
    IContainer container;

    public void Open(IContainer container)
    {
        if (this.container != null)
            this.container.OnChange -= FillList;
        this.container = container;
        FillList();
        container.OnChange += FillList;
    }

    void FillList()
    {
        var containerItems = container.GetItems(0, items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            if (i < containerItems.Length)
            {
                items[i].gameObject.SetActive(i < containerItems.Length);
                items[i].SetItem(containerItems[i]);
            }
            else
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }
}
