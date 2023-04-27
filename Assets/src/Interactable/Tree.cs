using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    public SpriteRenderer Trunk;
    public SpriteRenderer[] Leaves;
    public ParticleSystem ParticleSystem;

    public bool isChopped;

    public float MinimumDistance => 1f;
    public float TimeToComplete => 7f;
    public bool ValidTarget => isChopped == false;


    public Item reward;
    public int rewardLow = 1;
    public int rewardHigh = 2;


    void GetChopped(GameObject user)
    {
        Trunk.enabled = false;
        foreach (var leaf in Leaves)
            leaf.enabled = false;
        ParticleSystem.Play();
        isChopped = true;
        GetComponent<BoxCollider2D>().enabled = false;
        user.GetComponent<Inventory>().AddItem(reward, Random.Range(rewardLow, rewardHigh+1));
    }

    public void Interact(GameObject user, Vector3 worldPosition)
    {
        if(!isChopped)
            GetChopped(user);
    }

    public Vector2 GetInteractLocation(Vector3 where) => transform.position + new Vector3(0f, -.25f, 0f);
    public Vector2 GetInteractLocation() => transform.position + new Vector3(0f, -.25f, 0f);
}
