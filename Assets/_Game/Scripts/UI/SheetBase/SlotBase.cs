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
    UnityAction<SlotBase<Data>> actionCallBack;
    public Data slotData;
    public Button btnChoose;
    public Sequence sequence;
    public CanvasGroup cGroup;
    public Transform trsAnim;
    private void Awake()
    {
        btnChoose.onClick.AddListener(OnChoose);
    }

    public virtual void OnEnable() {
        trsAnim.localScale = UIAnimationController.vector08;
        cGroup.alpha = 0;
    }

    public virtual void InitData(Data slotData) {
        this.slotData = slotData;
    }

    public void SetActionCallback(UnityAction<SlotBase<Data>> actionCallBack) { this.actionCallBack = actionCallBack; }

    public virtual void OnChoose()
    {
        UIAnimationController.BasicButton(trsAnim, .25f, () => {
            if (actionCallBack != null)
                actionCallBack(this);
        });
    }

    public virtual void SelectedMode() { }
    public virtual void DeselectMode() { }
    public virtual void ReloadData() { }

    public virtual void PlaySequence() {
        sequence = UIAnimationController.SlotAnimation(trsAnim, .5f);
        sequence.Append(UIAnimationController.GroupAlphaAnimation(cGroup, 0, 1, .5f));
        sequence.Play();
    }

    public virtual void StopSequence() {
        sequence.Kill();
    }
}