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

    void Generate()
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
}
