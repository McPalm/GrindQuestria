using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface on containers to be readable and observable by UI and other things.
/// </summary>
public interface IContainer
{
    ItemBundle[] GetItems(int start, int length);
    int Count { get; }
    event System.Action OnChange;
}
