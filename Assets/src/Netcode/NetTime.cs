using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetTime : NetworkBehaviour
{
    public float TimeOfYear => SecondsIntoYear / YearDurationInSeconds;
    public float SecondsIntoYear => (float)(NetworkTime.time - StartTime) % YearDurationInSeconds;
    public float YearDurationInSeconds = 1320f;

    int month = -1;
    public string NameOfMonth { get; set; }

    [SyncVar]
    public double StartTime;

    private void Start()
    {
        StartTime = NetworkTime.time;
    }

    void Update()
    {
        if (month != (int)(TimeOfYear * 12f))
            UpdateMonth();
    }

    void UpdateMonth()
    {
        month = (int)(TimeOfYear * 12f);
        switch(month)
        {
            case 0: NameOfMonth = "Early Spring"; break;
            case 1: NameOfMonth = "Mid Spring"; break;
            case 2: NameOfMonth = "Late Spring"; break;
            case 3: NameOfMonth = "Early Summer"; break;
            case 4: NameOfMonth = "Mid Summer"; break;
            case 5: NameOfMonth = "Late Summer"; break;
            case 6: NameOfMonth = "Early Autumn"; break;
            case 7: NameOfMonth = "Mid Autumn"; break;
            case 8: NameOfMonth = "Late Autumn"; break;
            case 9: NameOfMonth = "Early Winter"; break;
            case 10: NameOfMonth = "Mid Winter"; break;
            case 11: NameOfMonth = "Late Winter"; break;
        }
    }
}
