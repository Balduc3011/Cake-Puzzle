using DG.Tweening;
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
        //FirstSelect();
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
        SelectNavItem(2);
    }

    void SelectNavItem(int index)
    {
        Debug.Log(index);
        selector.DOMove(navBarItems[index].position.position, 0.25f);
        if(selectedItem != null)
        {

        }
        selectedItem = navBarItems[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
