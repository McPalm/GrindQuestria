using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool ValidTarget { get; }
    float MinimumDistance { get; }
    void Interact(GameObject user, DoThing.ThingToDo info);
    float TimeToComplete(DoThing.ThingToDo info);
    Vector2 GetInteractLocation(Vector3 worldPosition);
    (bool canUse, string failmessage) ValidateUse(GameObject user, DoThing.ThingToDo info);
}

