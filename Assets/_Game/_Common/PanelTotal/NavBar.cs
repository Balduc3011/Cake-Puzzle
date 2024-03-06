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
        selectorRect = selector.GetComponent<RectTransform>();
        selectorRect.sizeDelta = new Vector2(rect.rect.width / navBarItems.Count, rect.rect.height);
        selector.localScale = new Vector3(1.2f, 1f, 1f);
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
        //SelectNavItem(2);
        selectedItem = navBarItems[2];
    }

    void SelectNavItem(int index)
    {
        if (selectedItem == navBarItems[index]) return;
        if(selectedItem != null)
        {
            selector.DOScaleX(1, 0.1f).OnComplete(
                () => {
                    selector.DOScaleX(1.2f, 0.15f);
                    navBarItems[index].OnSelect();
                }
                );
            selectedItem.OnDeselect();
            Sequence navSelectSequence = DOTween.Sequence();
            navSelectSequence.Append(selector.DOMove(selectedItem.lowPosition.position, 0.1f));
            navSelectSequence.Append(selector.DOMove(navBarItems[index].lowPosition.position, 0.01f));
            navSelectSequence.Append(selector.DOMove(navBarItems[index].position.position, 0.15f));
        }
        else
        {
            selector.position = navBarItems[index].position.position;
        }
        selectedItem = navBarItems[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
