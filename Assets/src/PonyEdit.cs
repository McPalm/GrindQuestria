using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PonyEdit : MonoBehaviour
{
    static bool firstTime = true;
    static public PonyGen.SerializedApperance savedApperance;

    void Start()
    {
        if(firstTime)
        {
            Model.Generate();
            savedApperance = Model.GetApperance();
        }
        else
            Model.SetApperance(savedApperance);
        firstTime = true;
    }

    public PonyGen Model;
    public void SetManeColor(string color)
    {
        var parsed = Parse(color);
        if(parsed != Color.clear)
        {
            Model.ManeColor = parsed;
            Model.Apply();
            savedApperance = Model.GetApperance();
        };
        
    }
    public void SetBodyColor(string color)
    {
        var parsed = Parse(color);
        if (parsed != Color.clear)
        {
            Model.BodyColor = parsed;
            Model.Apply();
            savedApperance = Model.GetApperance();
        };

    }
    public void SetTailColor(string color)
    {
        var parsed = Parse(color);
        if (parsed != Color.clear)
        {
            Model.TailColor = parsed;
            Model.Apply();
            savedApperance = Model.GetApperance();
        };

    }

    public void NextMane()
    {
        Model.ManeType++;
        Model.Apply();
        savedApperance = Model.GetApperance();
    }
    public void NextEye()
    {
        Model.EyeType++;
        Model.Apply();
        savedApperance = Model.GetApperance();
    }
    public void NextBody()
    {
        Model.BodyType++;
        Model.Apply();
        savedApperance = Model.GetApperance();
    }
    public void NextTail()
    {
        Model.TailType++;
        Model.Apply();
        savedApperance = Model.GetApperance();
    }

    Color Parse(string color)
    {
        if (ColorUtility.TryParseHtmlString(color, out var parsed))
        {
            parsed.a = 1f;
            return parsed;
        }
        else if (ColorUtility.TryParseHtmlString("#" + color, out parsed))
        {
            parsed.a = 1f;
            return parsed;
        }

        return Color.clear;
    }
}

