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
    DoThing.ThingToDo info;

    public float Progress => (float)((NetworkTime.time - startTime) / (endTime - startTime));

    public void InteractWith(IInteractable target, DoThing.ThingToDo info)
    {
        var validate = target.ValidateUse(gameObject, info);
        if (validate.canUse)
        {
            startTime = NetworkTime.time;
            endTime = NetworkTime.time + target.TimeToComplete(info);
            this.target = target;
            Busy = true;
            GetComponent<Animator>().SetBool("Working", true);
            this.info = info;
        }
        else
        {
            Debug.Log(validate.failmessage);
        }
    }

    void FixedUpdate()
    {
        if (isServer)
        {

            if (Busy && NetworkTime.time > endTime)
            {
                CompleteTask();
            }
            if (Busy && !target.ValidTarget)
            {
                Cancel();
            }
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
        target.Interact(gameObject, info);
        Busy = false;
    }
}
