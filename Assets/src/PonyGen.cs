using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PonyGen : MonoBehaviour
{
    public bool generate;

    public PonyCollection PonyCollection;

    public SpriteRenderer Body;
    public SpriteRenderer Mane;
    public SpriteRenderer Tail;
    public SpriteRenderer Eye;
    [Space(10)]
    public Color BodyColor;
    public Color ManeColor;
    public Color TailColor;
    [Space(10)]
    public int BodyType;
    public int ManeType;
    public int TailType;
    public int EyeType;

    // Start is called before the first frame update
    void Start()
    {
        if (generate)
            Generate();
        else
            Apply();
    }

    public void Generate()
    {
        BodyType = Random.Range(0, PonyCollection.BodyTypes.Length);
        ManeType = Random.Range(0, PonyCollection.ManeTypes.Length);
        TailType = Random.Range(0, PonyCollection.TailTypes.Length);
        EyeType = Random.Range(0, PonyCollection.EyeTypes.Length);
        BodyColor = Color.HSVToRGB(Random.value, Random.value * .8f, (Random.value + Random.value) * .3f + .6f);
        ManeColor = Color.HSVToRGB(Random.value, .2f + Random.value * .8f, (Random.value + Random.value) * .3f + .1f);
        TailColor = TailType == 2 ? BodyColor : ManeColor;
        Apply();
    }

    void Apply()
    {
        Body.color = BodyColor;
        Mane.color = ManeColor;
        Tail.color = TailColor;
        Body.sprite = PonyCollection.BodyTypes[BodyType];
        Mane.sprite = PonyCollection.ManeTypes[ManeType];
        Tail.sprite = PonyCollection.TailTypes[TailType];
        Eye.sprite = PonyCollection.EyeTypes[EyeType];
        Mane.transform.localPosition = PonyCollection.maneOffsets[BodyType] * .0625f;
        Tail.transform.localPosition = PonyCollection.tailOffsets[BodyType] * .0625f;
        Eye.transform.localPosition = PonyCollection.maneOffsets[BodyType] * .0625f;
    }

    public SerializedApperance GetApperance()
    {
        return new SerializedApperance()
        {
            body = BodyType,
            mane = ManeType,
            tail = TailType,
            eye = EyeType,
            body_r = BodyColor.r,
            body_g = BodyColor.g,
            body_b = BodyColor.b,
            mane_r = ManeColor.r,
            mane_g = ManeColor.g,
            mane_b = ManeColor.b,
            tail_r = TailColor.r,
            tail_g = TailColor.g,
            tail_b = TailColor.b,
        };
    }

    public void SetApperance(SerializedApperance sap)
    {
        BodyType = sap.body;
        ManeType = sap.mane;
        TailType = sap.tail;
        EyeType = sap.eye;
        BodyColor = new Color(sap.body_r, sap.body_g, sap.body_b);
        ManeColor = new Color(sap.mane_r, sap.mane_g, sap.mane_b);
        TailColor = new Color(sap.tail_r, sap.tail_g, sap.tail_b);
        Apply();
    }

    [System.Serializable]
    public struct SerializedApperance
    {
        public int body, mane, tail, eye;
        public float body_r, body_g, body_b, mane_r, mane_g, mane_b, tail_r, tail_g, tail_b;
    }
}
