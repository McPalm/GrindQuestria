using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Inventory : NetworkBehaviour
{
    const int SLOTS = 16;

    Item[] items;
    int[] stackSizes;

    public Item GetItem(int index) => items[index];
    public int GetStackSize(int index) => stackSizes[index];

    void Awake()
    {
        items = new Item[SLOTS];
        stackSizes = new int[SLOTS];
    }

    [Server]
    /// <returns>Number of items failed to go to inventory</returns>
    public int AddItem(Item item, int qty = 1)
    {
        int index = FirstNonFullStack(item);
        if (index == -1)
            index = FirstEmptyIndex();
        if (index == -1)
            return qty;
        items[index] = item;
        int add = Mathf.Min(qty, item.stackSize - stackSizes[index]);
        stackSizes[index] += add;
        qty -= add;
        SendAllItems();
        if (qty > 0)
            return AddItem(item, qty);
        return qty;
    }
    public bool HasItem(Item item) => FirstIndexOf(item) > -1;
    public int NumberOfItem(Item item)
    {
        int qty = 0;
        for (int i = 0; i < SLOTS; i++)
        {
            if (items[i] == item)
                qty += stackSizes[i];
        }
        return qty;
    }
    public int FirstIndexOf(Item item)
    {
        for(int i = 0; i < SLOTS; i++)
        {
            if (items[i] == item && stackSizes[i] > 0)
                return i;
        }
        return -1;
    }
    public int FirstNonFullStack(Item item)
    {
        for (int i = 0; i < SLOTS; i++)
        {
            if (items[i] == item && stackSizes[i] < item.stackSize)
                return i;
        }
        return -1;
    }
    public int FirstEmptyIndex()
    {
        for (int i = 0; i < SLOTS; i++)
        {
            if (stackSizes[i] == 0)
                return i;
        }
        return -1;
    }
    [Server]
    public int RemoveItem(Item item, int qty)
    {
        int removed = 0;
        for (int i = SLOTS-1; i >= 0 && removed < qty; i--)
        {
            if(items[i] == item)
            {
                int remove = Mathf.Min(qty - removed, stackSizes[i]);
                removed += remove;
                stackSizes[i] -= removed;
            };
        }
        SendAllItems();
        return removed;
    }

    void SendAllItems()
    {
        int[] indexes = new int[SLOTS];
        for (int i = 0; i < SLOTS; i++)
        {
            indexes[i] = ItemCollection.Instance.IndexOf(items[i]);
        }
        SendAllItems(indexes, stackSizes);
    }

    [ClientRpc]
    void SendAllItems(int[] itemIndexes, int[] stackSizes)
    {
        for (int i = 0; i < SLOTS; i++)
        {
            items[i] = ItemCollection.Instance.GetItem(itemIndexes[i]);
        }
        this.stackSizes = stackSizes;
    }
}
