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
            var json = PlayerPrefs.GetString("Apperance", "");
            if (json == "")
                Model.Generate();
            else
                Model.SetApperance(JsonUtility.FromJson<PonyGen.SerializedApperance>(json));
            savedApperance = Model.GetApperance();
        }
        else
            Model.SetApperance(savedApperance);
        firstTime = false;
    }

    public PonyGen Model;
    public void SetManeColor(string color)
    {
        var parsed = Parse(color);
        if(parsed != Color.clear)
        {
            Model.ManeColor = parsed;
            ApplyAndSave();
        };
        
    }
    public void SetBodyColor(string color)
    {
        var parsed = Parse(color);
        if (parsed != Color.clear)
        {
            Model.BodyColor = parsed;
            ApplyAndSave();
        };

    }
    public void SetTailColor(string color)
    {
        var parsed = Parse(color);
        if (parsed != Color.clear)
        {
            Model.TailColor = parsed;
            ApplyAndSave();
        };

    }

    public void NextMane()
    {
        Model.ManeType++;
        ApplyAndSave();
    }
    public void NextEye()
    {
        Model.EyeType++;
        ApplyAndSave();
    }
    public void NextBody()
    {
        Model.BodyType++;
        ApplyAndSave();
    }
    public void NextTail()
    {
        Model.TailType++;
        ApplyAndSave();
    }

    void ApplyAndSave()
    {
        Model.Apply();
        savedApperance = Model.GetApperance();
        PlayerPrefs.SetString("Apperance", JsonUtility.ToJson(savedApperance));
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

