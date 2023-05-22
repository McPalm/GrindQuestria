using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameMessages : NetworkBehaviour
{
    static public GameMessages Instance;

    private void Awake()
    {
        Instance = this;
    }

    public TextMeshProUGUI MessageText;

    public void FeedbackMessage(GameObject target, string message)
    {
        var id = target.GetComponent<NetCharacterID>();
        if(id != null)
        {
            TargetFeedbackMessage(id.Connection, message);
        }
    }

    [TargetRpc(channel = Channels.Reliable)]
    public void TargetFeedbackMessage(NetworkConnection connectionToClient, string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessage(message));
    }

    IEnumerator ShowMessage(string message)
    {
        MessageText.color = new Color(1f, 1f, 1f);
        MessageText.gameObject.SetActive(true);
        MessageText.text = message;
        yield return new WaitForSeconds(.2f);
        MessageText.color = new Color(1f, 1f, 1f, .8f);
        yield return new WaitForSeconds(1f + message.Length * .03f);
        for(float f = 0f; f < 1f; f += Time.deltaTime)
        {
            MessageText.color = new Color(1f, 1f, 1f, .8f - f * .8f);
            yield return null;
        }
        MessageText.gameObject.SetActive(false);
    }
}
