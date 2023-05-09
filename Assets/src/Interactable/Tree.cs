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
    public float TimeToComplete => 7f;
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
        ParticleSystem.Play();
    }

    public void Interact(GameObject user, Vector3 worldPosition)
    {
        if(Alive)
            GetChopped(user);
    }

    public Vector2 GetInteractLocation(Vector3 where) => transform.position + new Vector3(0f, -.25f, 0f);
}
