using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NavBarItem : MonoBehaviour
{
    [SerializeField] Button itemBtn;
    [SerializeField] Image iconImg;
    public Transform position;
    public Transform lowPosition;
    [SerializeField] Transform iconTrs;
    UnityAction navBarCallBack;

    void Start()
    {
        itemBtn.onClick.AddListener(ButtonOnClick);
    }

    public void SetupButton(UnityAction callBack)
    {
        navBarCallBack = callBack;
    }

    void ButtonOnClick()
    {
        if(navBarCallBack != null) {  navBarCallBack(); }
    }

    public void OnSelect()
    {
        iconTrs.DOScale(1.2f, 0.1f);
    }
    public void OnDeselect()
    {
        iconTrs.DOScale(1, 0.1f);
    }
}
