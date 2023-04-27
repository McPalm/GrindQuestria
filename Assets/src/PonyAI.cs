using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PonyAI : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var doThing = GetComponent<DoThing>();
        yield return new WaitForSeconds(.5f + Random.value * 5f);
        for(; ;)
        {
            if(doThing.Idle && Random.value < .5f)
            {
                if (Random.value < 0f)
                {
                    var tree = PunchTree();
                    if (tree != null)
                    {
                        doThing.QueueThingToDo(new DoThing.ThingToDo()
                        {
                            what = DoThing.Things.interact,
                            who = tree
                        });
                        yield return new WaitForSeconds(7f);
                    }
                }
                else
                {
                    doThing.QueueThingToDo(new DoThing.ThingToDo()
                    {
                        what = DoThing.Things.walkhere,
                        where = new Vector2(-12f + Random.value * 24f, -10 + Random.value * 20f),
                    });
                }
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(2f + Random.value * 5f);
        }
    }

    GameObject PunchTree()
    {
        var trees = Physics2D.CircleCastAll(transform.position, 5f, Vector2.zero, 0f, LayerMask.GetMask("Interactable"));
        GameObject bestHit = null;
        foreach(var hit in trees)
        {
            if (bestHit == null)
                bestHit = hit.transform.gameObject;
            else if((transform.position - bestHit.transform.position).sqrMagnitude > (transform.position - hit.transform.position).sqrMagnitude)
                bestHit = hit.transform.gameObject;
        }
        return bestHit;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
