using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGetPopupUI : MonoBehaviour
{

    static public ItemGetPopupUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public GameObject ItemBundleUIParent;

    public void ShowItemGet(params ItemBundle[] itemBundles)
    {
        foreach (ItemBundle bundle in itemBundles)
            StartCoroutine(Show(bundle));
    }

    IEnumerator Show(ItemBundle bundle)
    {
        var widget = GetNextItemBundleUI();
        widget.SetItem(bundle);
        widget.gameObject.SetActive(true);
        widget.transform.SetAsFirstSibling();
        yield return new WaitForSeconds(5f);
        widget.gameObject.SetActive(false);
    }

    ItemBundleUI GetNextItemBundleUI()
    {
        var list = ItemBundleUIParent.GetComponentsInChildren<ItemBundleUI>(true);
        return list[list.Length - 1];
    }
}
