using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public LayerMask InteractionLayer;
    public NetInput netInput;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var clickedInteractable = Physics2D.Raycast(clickPos, Vector2.zero, 0f, InteractionLayer);
            if (clickedInteractable)
            {
                var interactable = clickedInteractable.transform.GetComponent<IInteractable>();
                if (interactable != null)
                    netInput.DoThing(new DoThing.ThingToDo()
                    {
                        what = DoThing.Things.interact,
                        who = clickedInteractable.transform.gameObject,
                        where = clickPos,
                    });
                else
                    netInput.DoThing(new DoThing.ThingToDo()
                    {
                        what = DoThing.Things.interact,
                        who = Shops.Instance.gameObject,
                        where = clickPos,
                    });
            }
            else
            {
                netInput.DoThing(new DoThing.ThingToDo()
                {
                    what = DoThing.Things.walkhere,
                    where = clickPos,
                });
            }
        }
    }
}
