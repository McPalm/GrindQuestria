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

    public void OpenRecepie(Recepie recepie, System.Action onClick)
    {
        OnClick = onClick;
        displayName.text = recepie.Product.item.displayName;
        productIcon.sprite = recepie.Product.item.sprite;
        for(int i = 0; i < materialIcon.Length; i++)
        {
            if(i < recepie.Ingredients.Length)
            {
                materialIcon[i].gameObject.SetActive(true);
                materialQTY[i].gameObject.SetActive(true);
                materialIcon[i].sprite = recepie.Ingredients[i].item.sprite;
                materialQTY[i].text = $"x{recepie.Ingredients[i].qty}";
            }
            else
            {
                materialIcon[i].gameObject.SetActive(false);
                materialQTY[i].gameObject.SetActive(false);
            }
        }
    }
}
