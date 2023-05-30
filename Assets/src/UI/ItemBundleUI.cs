using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemBundleUI : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Quantity;
    public Image Image;

    public void SetItem(ItemBundle itemBundle)
    {
        if (Name)
            Name.text = itemBundle.item.displayName;
        if (Quantity)
            Quantity.text = $"x{itemBundle.qty}";
        if (Image)
            Image.sprite = itemBundle.item.sprite;
    }
}
