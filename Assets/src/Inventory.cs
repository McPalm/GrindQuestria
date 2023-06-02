using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Inventory : NetworkBehaviour, IContainer
{
    public event Action OnChange;
    public int Count => container.Count;
    Container container;

    private void Start()
    {
    if(isServer)
        {
            container = Containers.Instance.InventoryOf(gameObject);
        }
    }

    [Server]
    /// <returns>Number of items failed to go to inventory</returns>
    public int AddItem(Item item, int qty = 1)
    {
        throw new Exception("Obsolete");
    }

    public bool HasItem(Item item) => container.HasItems(new ItemBundle(item, 1));
    public bool HasBundle(ItemBundle bundle) => container.HasItems(bundle);
    public bool HasBundles(ItemBundle[] bundles) => container.HasItems(bundles);
    public void RemoveBundles(params ItemBundle[] bundles) => container.RemoveItems(bundles);
    public void AddBundles(params ItemBundle[] bundles) => container.AddItems(bundles);
    public ItemBundle[] GetItems(int start, int length) => container.GetItems(start, length);
}
