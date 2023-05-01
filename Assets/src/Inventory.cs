using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Inventory : NetworkBehaviour
{
    Item[] items;
    int[] stacksizes;

    public Item GetItem(int index) => items[index];
    public int GetStackSize(int index) => stacksizes[index];

    void Awake()
    {
        items = new Item[16];
        stacksizes = new int[16];
    }

    /// <returns>Number of items failed to go to inventory</returns>
    public int AddItem(Item item, int qty = 1)
    {
        int index = FirstNonFullStack(item);
        if (index == -1)
            index = FirstEmptyIndex();
        if (index == -1)
            return qty;
        items[index] = item;
        int add = Mathf.Min(qty, item.stackSize - stacksizes[index]);
        stacksizes[index] += add;
        qty -= add;

        if (qty > 0)
            return AddItem(item, qty);
        return qty;
    }

    public bool HasItem(Item item) => FirstIndexOf(item) > -1;
    public int NumberOfItem(Item item)
    {
        int qty = 0;
        for (int i = 0; i < 16; i++)
        {
            if (items[i] == item)
                qty += stacksizes[i];
        }
        return qty;
    }
    public int FirstIndexOf(Item item)
    {
        for(int i = 0; i < 16; i++)
        {
            if (items[i] == item && stacksizes[i] > 0)
                return i;
        }
        return -1;
    }
    public int FirstNonFullStack(Item item)
    {
        for (int i = 0; i < 16; i++)
        {
            if (items[i] == item && stacksizes[i] < item.stackSize)
                return i;
        }
        return -1;
    }
    public int FirstEmptyIndex()
    {
        for (int i = 0; i < 16; i++)
        {
            if (stacksizes[i] == 0)
                return i;
        }
        return -1;
    }

    public int RemoveItem(Item item, int qty)
    {
        int removed = 0;
        for (int i = 15; i >= 0 && removed < qty; i--)
        {
            if(items[i] == item)
            {
                int remove = Mathf.Min(qty - removed, stacksizes[i]);
                removed += remove;
                stacksizes[i] -= removed;
            };
        }
        return removed;
    }

    void SendAllItems()
    {
        string[] names = new string[items.Length];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = items[i].name;
        }
        SendAllItems(names, stacksizes);
    }

    [ClientRpc]
    void SendAllItems(string[] items, int[] stackSizes)
    {

    }
}
