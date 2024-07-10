using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NavBarItem : MonoBehaviour
{
    [SerializeField] NavBar navBar;
    [SerializeField] Button itemBtn;
    public RectTransform rectTransform;
    [SerializeField] Image iconImg;
    [SerializeField] Image bg;
    [SerializeField] Sprite selectedBG;
    [SerializeField] Sprite unSelectedBG;
    public Transform position;
    public Transform lowPosition;
    [SerializeField] Transform iconTrs;
    UnityAction navBarCallBack;

    void Start()
    {
        //itemBtn.onClick.AddListener(ButtonOnClick);
    }

    public void SetupButton(UnityAction callBack)
    {
        navBarCallBack = callBack;
    }

    public void ButtonOnClick(int index = -1)
    {
        Debug.Log(index);
        if (index == -1)
        {
            if (navBarCallBack != null) { navBarCallBack(); }
        }
        else
        {
            if(navBar != null)
                navBar.SelectNavItem(index);
        }
    }

    public void OnSelect()
    {
        iconTrs.DOScale(1.45f, 0.3f).SetEase(Ease.InOutBack);
        bg.sprite = selectedBG;
    }
    public void OnDeselect()
    {
        iconTrs.DOScale(1, 0.25f).SetEase(Ease.OutBack);
        bg.sprite = unSelectedBG;
    }
}
