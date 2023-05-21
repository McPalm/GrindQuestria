using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICraftMenuItem 
{
    string ProductName { get; }
    Sprite ProductSprite { get; }
    IEnumerable<ItemBundle> Materials { get; }
}
