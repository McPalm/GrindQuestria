using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemListUI : MonoBehaviour
{
    public ItemBundleUI[] items;
    IContainer container;

    public event System.Action<Item> leftClick;
    public event System.Action<Item> rightClick;

    static GraphicRaycaster m_Raycaster;
    static PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private void Start()
    {
        m_EventSystem = EventSystem.current;
        m_Raycaster = EventSystem.FindObjectOfType<GraphicRaycaster>();
    }

    public void Open(IContainer container)
    {
        if (this.container != null)
            this.container.OnChange -= FillList;
        this.container = container;
        FillList();
        container.OnChange += FillList;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            foreach (RaycastResult result in results)
            {
                var itemUI = result.gameObject.GetComponent<ItemBundleUI>();
                if(itemUI != null)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (itemUI == items[i])
                        {
                            if (Input.GetMouseButtonDown(0))
                                leftClick?.Invoke(container.GetItems(0, 11)[i].item);
                            else
                                rightClick?.Invoke(container.GetItems(0, 11)[i].item);
                        }
                    }
                }
            }
        }
    }

    void FillList()
    {
        var containerItems = container.GetItems(0, items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            if (i < containerItems.Length)
            {
                items[i].gameObject.SetActive(i < containerItems.Length);
                items[i].SetItem(containerItems[i]);
            }
            else
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }
}
