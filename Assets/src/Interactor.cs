using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : NetworkBehaviour
{
    [SyncVar]
    public bool Busy;
    IInteractable target;
    [SyncVar]
    double startTime;
    [SyncVar]
    double endTime;
    Vector3 where;

    public float Progress => (float)((NetworkTime.time - startTime) / (endTime - startTime));

    public void InteractWith(IInteractable target, Vector3 worldPosition)
    {
        if (target.ValidTarget)
        {
            startTime = NetworkTime.time;
            endTime = NetworkTime.time + target.TimeToComplete;
            this.target = target;
            Busy = true;
            GetComponent<Animator>().SetBool("Working", true);
            where = worldPosition;
        }
    }

    [Server]
    void FixedUpdate()
    {
        if(Busy && NetworkTime.time > endTime)
        {
            CompleteTask();
        }
        if(Busy && !target.ValidTarget)
        {
            Cancel();
        }
    }

    public void Cancel()
    {
        GetComponent<Animator>().SetBool("Working", false);
        target = null;
        Busy = false;
    }

    void CompleteTask()
    {
        GetComponent<Animator>().SetBool("Working", false);
        target.Interact(gameObject, where);
        Busy = false;
    }
}
