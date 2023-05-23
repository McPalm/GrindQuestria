using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class BreakableTile : RuleTile
{
    public float breakTime = 7f;
    public string displayName;
    public ItemBundle[] Drops;
    public Skills skill;
    public TileBase breaksInto;
}
