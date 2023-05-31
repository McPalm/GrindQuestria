using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageUI : MonoBehaviour
{
    public ItemListUI LeftList;
    public ItemListUI RightList;
    public GameObject Background;

    public void Open(IContainer leftContainer, IContainer rightContainer, System.Action<ItemBundle> MoveRight, System.Action<ItemBundle> MoveLeft)
    {
        Background.SetActive(true);
        LeftList.Open(leftContainer);
        RightList.Open(rightContainer);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Background.activeSelf && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Background.SetActive(false);
        }
    }
}
