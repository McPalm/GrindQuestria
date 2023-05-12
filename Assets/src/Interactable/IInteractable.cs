using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool ValidTarget { get; }
    float MinimumDistance { get; }
    void Interact(GameObject user, Vector3 worldPosition);
    float TimeToComplete(Vector3 worldPosition);
    Vector2 GetInteractLocation(Vector3 worldPosition);
}
