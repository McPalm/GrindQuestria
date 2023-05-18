using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CraftMenuUI : MonoBehaviour
{
    public CraftMenuItemUI MenuItemPrefab;
    public GameObject Background;
    public Transform MenuItemParent;
    [SerializeField]
    List<CraftMenuItemUI> menuItemUIs;
    public TextMeshProUGUI shopLabel;

    public void Open(ShopData shop)
    {
        gameObject.SetActive(true);
        Background.SetActive(true);
        shopLabel.text = shop.displayName;
        int count = Mathf.Max(shop.Recepies.Length, menuItemUIs.Count);
        for (int i = 0; i < count; i++)
        {
            var menuItem = GetOrCreate(i);
            if (i < shop.Recepies.Length)
                menuItem.OpenRecepie(shop.Recepies[i], null);
            else
                menuItem.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(Background.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
            if (Input.GetMouseButtonDown(0) & !EventSystem.current.IsPointerOverGameObject())
                Close();
        }
    }

    CraftMenuItemUI GetOrCreate(int index)
    {
        if (index < menuItemUIs.Count)
            return menuItemUIs[index];
        var menuItem = Instantiate(MenuItemPrefab, MenuItemParent);
        menuItemUIs.Add(menuItem);
        return menuItem;
    }

    public void Close()
    {
        Background.SetActive(false);
    }
}
