using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalResources : MonoBehaviour, IInteractable
{
    public bool ValidTarget => throw new System.NotImplementedException();

    public float MinimumDistance => throw new System.NotImplementedException();

    public Vector2 GetInteractLocation(Vector3 worldPosition)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(GameObject user, DoThing.ThingToDo info)
    {
        throw new System.NotImplementedException();
    }

    public float TimeToComplete(DoThing.ThingToDo info)
    {
        throw new System.NotImplementedException();
    }

    public (bool canUse, string failmessage) ValidateUse(GameObject user, DoThing.ThingToDo info)
    {
        throw new System.NotImplementedException();
    }
}
