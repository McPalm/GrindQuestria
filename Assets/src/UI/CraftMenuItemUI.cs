using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftMenuItemUI : MonoBehaviour
{
    public TextMeshProUGUI displayName;
    public Image productIcon;
    public Image[] materialIcon;
    public TextMeshProUGUI[] materialQTY;

    event System.Action OnClick;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ClickListenerer);
    }

    void ClickListenerer() => OnClick?.Invoke();

    public void OpenRecepie(ICraftMenuItem recepie, System.Action onClick)
    {
        OnClick = onClick;
        displayName.text = recepie.Product.item.displayName;
        productIcon.sprite = recepie.Product.item.sprite;
        foreach (var icon in materialIcon)
            icon.gameObject.SetActive(false);
        foreach (var qty in materialQTY)
            qty.gameObject.SetActive(false);
        int count = 0;
        foreach (var material in recepie.Materials)
        {
            materialIcon[count].gameObject.SetActive(true);
            materialQTY[count].gameObject.SetActive(true);
            materialIcon[count].sprite = material.item.sprite;
            materialQTY[count].text = $"x{material.qty}";
            count++;
        }
    }
}
