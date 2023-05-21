using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recepie : ScriptableObject, ICraftMenuItem
{
    public float craftTime = 8f;
    public ItemBundle Product;
    public ItemBundle[] Materials;

    public string ProductName => Product.qty == 1 ? Product.item.displayName : $"{Product.item.displayName} x{Product.qty}";
    public Sprite ProductSprite => Product.item.sprite;

    IEnumerable<ItemBundle> ICraftMenuItem.Materials => Materials;
}
