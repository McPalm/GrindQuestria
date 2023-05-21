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

    public void Open(ICraftMenuClient shop, System.Action<int> OnClick)
    {
        gameObject.SetActive(true);
        Background.SetActive(true);
        shopLabel.text = shop.CraftMenuTitle;
        int count = 0;
        foreach(var item in shop.Items)
        {
            var menuItem = GetOrCreate(count);
            menuItem.gameObject.SetActive(true);
            int capture = count;
            menuItem.OpenRecepie(item, () => OnClick(capture));
            count++;
        }
        for(int i = count; i < menuItemUIs.Count; i++)
            menuItemUIs[i].gameObject.SetActive(false);
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
