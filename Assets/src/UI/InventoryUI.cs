using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public ItemListUI ItemList;
    public Inventory target;
    public GameObject background;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (background.activeSelf)
                background.SetActive(false);
            else
            {
                background.SetActive(true);
                ItemList.Open(target);
            }
        }
    }
}
