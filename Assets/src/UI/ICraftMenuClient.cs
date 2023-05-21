using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICraftMenuClient
{
    string CraftMenuTitle { get; }
    IEnumerable<ICraftMenuItem> Items { get; }
}
