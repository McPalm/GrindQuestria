using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recepie : ScriptableObject
{
    public Item Produce;
    public int ProduceQTY;
    public Item[] Ingredients;
    public int[] IngredientsQTY;
}
