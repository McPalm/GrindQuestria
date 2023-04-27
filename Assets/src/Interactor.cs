using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public bool Busy => target != null;
    IInteractable target;
    float startTime;
    float endTime;
    Vector3 where;


    public float Progress => (Time.timeSinceLevelLoad - startTime) / (endTime - startTime);

    public void InteractWith(IInteractable target, Vector3 worldPosition)
    {
        if (target.ValidTarget)
        {
            startTime = Time.timeSinceLevelLoad;
            endTime = Time.timeSinceLevelLoad + target.TimeToComplete;
            this.target = target;
            GetComponent<Animator>().SetBool("Working", true);
            where = worldPosition;
        }
    }

    void FixedUpdate()
    {
        if(Busy && Time.timeSinceLevelLoad > endTime)
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
    }

    void CompleteTask()
    {
        GetComponent<Animator>().SetBool("Working", false);
        target.Interact(gameObject, where);
        target = null;
    }
}
