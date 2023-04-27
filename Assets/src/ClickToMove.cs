using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public Vector2 goal;
    public LayerMask InteractionLayer;

    void Start()
    {
        goal = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var clickedInteractable = Physics2D.Raycast(clickPos, Vector2.zero, 0f, InteractionLayer);
            if (clickedInteractable)
            {
                GetComponent<DoThing>().DoThingImmediately(new DoThing.ThingToDo()
                {
                    what = DoThing.Things.interact,
                    who = clickedInteractable.transform.gameObject,
                    where = clickPos,
                });

            }
            else
                GetComponent<DoThing>().DoThingImmediately(new DoThing.ThingToDo()
                {
                    what = DoThing.Things.walkhere,
                    where = clickPos,
                });
        }
        
    }
}
