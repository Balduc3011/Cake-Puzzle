using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavBar : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] List<NavBarItem> navBarItems;
    [SerializeField] Transform selector;
    [SerializeField] RectTransform selectorRect;
    NavBarItem selectedItem;
    float selectedWidth;
    float nonSelectedWidth;

    void Start()
    {
        SetUp();
        FirstSelect();
    }

    void SetUp()
    {
        SetUpSelector();
        SetUpButton();
    }

    void SetUpSelector()
    {
        rect = GetComponent<RectTransform>();
        selectedWidth = (rect.rect.width / 5) + 100;
        nonSelectedWidth = (rect.rect.width / 5) - 25;
        selectorRect.sizeDelta = new Vector2(selectedWidth, selectorRect.sizeDelta.y);
    }

    void SetUpButton()
    {
        for (int i = 0; i < navBarItems.Count; i++)
        {
            int index = i;
            navBarItems[i].SetupButton(() => { SelectNavItem(index); });
        }
    }

    void FirstSelect()
    {
        SelectNavItem(2);
    }

    public void SelectNavItem(int index)
    {
        Debug.Log($"Select {index}");
        if (selectedItem == navBarItems[index]) return;
        if(selectedItem != null)
        {
            selectedItem.OnDeselect();
        }
        selectedItem = navBarItems[index];
        selectedItem.OnSelect();
        for (int i = 0; i < navBarItems.Count; i++)
        {
            if(i != index)
            {
                navBarItems[i].rectTransform.sizeDelta = new Vector2(nonSelectedWidth, selectedItem.rectTransform.sizeDelta.y);
            }
        }
        selectedItem.rectTransform.sizeDelta = new Vector2(selectedWidth, selectedItem.rectTransform.sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {
        selector.position = selectedItem.position.position;
    }
}
