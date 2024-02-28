using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UIAnimation;
using DG.Tweening;
public class SlotBase<Data> : MonoBehaviour
{
    public Data data;
    public Transform trsAnim;
    public Button btnChoose;
    UnityAction<SlotBase<Data>> actionCallback;
    private void Awake()
    {
        btnChoose.onClick.AddListener(OnChoose);
    }

    public virtual void InitData(Data data)
    {
        this.data = data;
    }

    public void SetActionCallback(UnityAction<SlotBase<Data>> actionCallback)
    {
        this.actionCallback = actionCallback;
    }

    public virtual void OnChoose()
    {
        UIAnimationController.BtnAnimZoomBasic(trsAnim, .25f, () => {
            if (actionCallback != null) actionCallback(this);
        });
    }
}