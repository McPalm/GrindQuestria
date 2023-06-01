using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageUI : MonoBehaviour
{
    public ItemListUI LeftList;
    public ItemListUI RightList;
    public GameObject Background;

    System.Action<ItemBundle> MoveRight;
    System.Action<ItemBundle> MoveLeft;

    private void Start()
    {
        LeftList.rightClick += LeftList_rightClick;
        LeftList.leftClick += LeftList_leftClick;
        RightList.rightClick += RightList_rightClick;
        RightList.leftClick += RightList_leftClick;
    }

    private void RightList_leftClick(Item item)
    {
        MoveLeft(new ItemBundle()
        {
            item = item,
            qty = 1
        });
    }

    private void RightList_rightClick(Item item)
    {
        MoveLeft(new ItemBundle()
        {
            item = item,
            qty = 999
        });
    }

    private void LeftList_leftClick(Item item)
    {
        MoveRight(new ItemBundle()
        {
            item = item,
            qty = 1
        });
    }

    private void LeftList_rightClick(Item item)
    {
        MoveRight(new ItemBundle()
        {
            item = item,
            qty = 999
        });
    }

    public void Open(IContainer leftContainer, IContainer rightContainer, System.Action<ItemBundle> MoveRight, System.Action<ItemBundle> MoveLeft)
    {
        Background.SetActive(true);
        LeftList.Open(leftContainer);
        RightList.Open(rightContainer);
        this.MoveRight = MoveRight;
        this.MoveLeft = MoveLeft;
    }



    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Background.activeSelf && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Background.SetActive(false);
        }
    }
}
