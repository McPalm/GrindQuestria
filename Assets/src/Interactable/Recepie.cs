using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recepie : ScriptableObject, ICraftMenuItem
{
    public float craftTime = 8f;
    public ItemBundle Product;
    public ItemBundle[] Materials;

    ItemBundle ICraftMenuItem.Product => Product;

    IEnumerable<ItemBundle> ICraftMenuItem.Materials => Materials;
}
