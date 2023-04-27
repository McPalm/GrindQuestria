using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskProgressBar : MonoBehaviour
{
    public Interactor Target;
    public ProgressBar bar;

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            bar.gameObject.SetActive(Target.Busy);
            bar.SetProgress(Target.Progress);
        }
    }
}
