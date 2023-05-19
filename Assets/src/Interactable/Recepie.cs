using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recepie : ScriptableObject
{
    public float craftTime = 8f;
    public ItemBundle Product;
    public ItemBundle[] Materials;
}
