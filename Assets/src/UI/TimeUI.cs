using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public NetTime NetTime;
    public Transform SpinDisk;
    public TextMeshProUGUI SeasonText;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (NetTime == null)
        {
            NetTime = FindObjectOfType<NetTime>(true);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpinDisk.localRotation = Quaternion.Euler(0f, 0f, NetTime.TimeOfYear * 360f - 90f);
        SeasonText.text = NetTime.NameOfMonth;
    }
}
