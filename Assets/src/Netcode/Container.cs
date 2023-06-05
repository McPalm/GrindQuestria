using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : IContainer
{
    public int maxSize = 8;

    public int Count => Items.Count;
    public event Action OnChange;
    public event Action<ItemBundle[]> OnAdd;


    private List<ItemBundle> Items;

    public Container(int size)
    {
        this.maxSize = size;
        Items = new List<ItemBundle>();
    }

    public ItemBundle[] GetItems(int start, int length) => Items.ToArray();

    public (bool success, ItemBundle[] leftovers) AddItems(params ItemBundle[] itemBundles)
    {
        foreach(var bundle in itemBundles)
        {
            var index = IndexOf(bundle.item);
            if (index == -1)
                Items.Add(bundle);
            else
            {
                Items[index] = new ItemBundle()
                {
                    item = bundle.item,
                    qty = bundle.qty + Items[index].qty,
                };
            }
        }
        OnChange?.Invoke();
        OnAdd?.Invoke(itemBundles);
        return (true, null);
    }

    public (bool success, ItemBundle[] actuallyRemoved) RemoveItems(params ItemBundle[] itemBundles)
    {
        List<ItemBundle> actuallyRemoved = new List<ItemBundle>();
        foreach (var bundle in itemBundles)
        {
            var index = IndexOf(bundle.item);
            if (index >= 0)
            {
                var remov = new ItemBundle()
                {
                    item = bundle.item,
                    qty = Items[index].qty < bundle.qty ? Items[index].qty : bundle.qty // lowest value of request and held
                };
                Items[index] = new ItemBundle()
                {
                    item = Items[index].item,
                    qty = Items[index].qty - remov.qty,
                };
                actuallyRemoved.Add(remov);
                if (Items[index].qty <= 0)
                    Items.RemoveAt(index);
            }
        }
        OnChange?.Invoke();
        return (true, actuallyRemoved.ToArray());
    }

    public bool HasItems(params ItemBundle[] itemBundles)
    {
        foreach (var bundle in itemBundles)
        {
            var index = IndexOf(bundle.item);
            if (index == -1)
                return false;
            if (Items[index].qty < bundle.qty)
                return false;
        }
        return true;
    }

    public int NumberOf(Item item)
    {
        var index = IndexOf(item);
        if (index < 0)
            return 0;
        return Items[index].qty;
    }

    int IndexOf(Item item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].item == item)
                return i;
        }
        return -1;
    }

    public ItemBundle.SerializedBundle[] Serialized()
    {
        Debug.LogWarning("Untested code");
        var ret = new ItemBundle.SerializedBundle[Items.Count];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = Items[i].Serialized();
        }
        return ret;
    }

    public void AdjustFromSerialized(ItemBundle.SerializedBundle[] serializedBundle)
    {
        Debug.LogWarning("Untested code");
        var deserialized = new ItemBundle[serializedBundle.Length];
        for (int i = 0; i < deserialized.Length; i++)
        {
            deserialized[i] = ItemBundle.Create(serializedBundle[i]);
        }

        foreach(var item in deserialized)
        {
            var adjust = item.qty - NumberOf(item.item);
            if (adjust > 0)
                AddItems(new ItemBundle(item.item, adjust));
            else if (adjust < 0)
                RemoveItems(new ItemBundle(item.item, adjust));
        }
        if (serializedBundle.Length < Items.Count)
        {
            // we gotta remove something
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                bool missing = true;
                for (int j = 0; j < deserialized.Length; j++)
                {
                    if (Items[i].item == deserialized[j].item)
                    {
                        missing = false;
                        continue;
                    }
                }
                if (missing)
                    Items.RemoveAt(i);
            }
        }
    }
}
