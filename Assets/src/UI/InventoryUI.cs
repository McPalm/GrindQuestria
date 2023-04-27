using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory target;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            for (int i = 0; i < 16; i++)
            {
                int qty = target.GetStackSize(i);
                Item item = target.GetItem(i);
                transform.GetChild(i).gameObject.SetActive(target.GetStackSize(i) > 0);
                if (qty > 0)
                {
                    transform.GetChild(i).GetComponent<Image>().sprite = item.sprite;
                    transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = $"x{qty}";
                }
            }
        }
    }
}
