using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public RectTransform bar;
    public RectTransform background;

    public void SetProgress(float value)
    {
        bar.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, value * background.rect.width);
    }
}
