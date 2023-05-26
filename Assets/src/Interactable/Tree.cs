using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Tree : NetworkBehaviour, IInteractable
{
    public SpriteRenderer Trunk;
    public SpriteRenderer[] Leaves;
    public ParticleSystem ParticleSystem;

    [SyncVar(hook ="SetChop")]
    public bool Alive;

    public float MinimumDistance => 1f;
    public float TimeToComplete(DoThing.ThingToDo info) => 7f;
    public bool ValidTarget => Alive;


    public Item reward;
    public int rewardLow = 1;
    public int rewardHigh = 2;

    void Start()
    {
        SetChop(false, Alive);
    }

    void SetChop(bool oldValue, bool alive)
    {
        Trunk.enabled = alive;
        GetComponent<BoxCollider2D>().enabled = alive;
        foreach (var leaf in Leaves)
            leaf.enabled = alive;
    }

    void GetChopped(GameObject user)
    {
        Alive = false;
        ChopEffects();
        user.GetComponent<Inventory>().AddItem(reward, Random.Range(rewardLow, rewardHigh+1));
    }

    [ClientRpc]
    void ChopEffects()
    {
        if(ParticleSystem != null)
            ParticleSystem.Play();
    }

    public void Interact(GameObject user, DoThing.ThingToDo info)
    {
        if(Alive)
            GetChopped(user);
    }

    public Vector2 GetInteractLocation(Vector3 where) => throw new System.Exception("Obsolete function");

    public (bool canUse, string failmessage) ValidateUse(GameObject user, DoThing.ThingToDo info)
    {
        Debug.LogWarning("Deprechiated class");
        return (true, null);
    }
}
