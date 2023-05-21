using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICraftMenuItem 
{
    ItemBundle Product { get; }
    IEnumerable<ItemBundle> Materials { get; }
}
