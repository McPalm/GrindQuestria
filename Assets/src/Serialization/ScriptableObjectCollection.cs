using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectCollection<T> : ScriptableObject where T : ScriptableObject
{
    public List<T> List;
}
