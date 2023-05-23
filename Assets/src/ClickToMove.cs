using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToMove : MonoBehaviour
{
    public LayerMask InteractionLayer;
    public NetInput netInput;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            var clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (NaturalResources.Instance.HasResourcesHere(clickPos))
            {
                Interact(NaturalResources.Instance, clickPos, NaturalResources.Instance.gameObject);
                return;
            }
            if (Shops.Instance.HasShopHere(clickPos))
            {
                WalkHere(clickPos);
                netInput.OpenShop(clickPos);
                return;
            }
            var clickedInteractable = Physics2D.Raycast(clickPos, Vector2.zero, 0f, InteractionLayer);
            if (clickedInteractable)
            {
                var interactable = clickedInteractable.transform.GetComponent<IInteractable>();
                Interact(interactable, clickPos, clickedInteractable.transform.gameObject);
                return;
            }

            WalkHere(clickPos);
        }
    }

    void Interact(IInteractable interactable, Vector2 clickPos, GameObject gameObject)
    {
        netInput.DoThing(new DoThing.ThingToDo()
        {
            what = DoThing.Things.interact,
            who = gameObject,
            where = clickPos,
        });
    }

    void WalkHere(Vector2 clickPos)
    {
        netInput.DoThing(new DoThing.ThingToDo()
        {
            what = DoThing.Things.walkhere,
            where = clickPos,
        });
    }
}
