using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEditor.UIElements;

public class SlotOrder : SlotBase<CakeData>
{
    [SerializeField] Image imgIcon;
    [SerializeField] Image imgCheck;
    [SerializeField] TextMeshProUGUI txtCountCake;
    [SerializeField] GameObject objCountCakeWrap;
    [SerializeField] Vector3 vectorPunch = new Vector3(.5f, .5f, .5f);
    int currentCakeDone;
    int totalCakeRequire;
    int orderIndex = -1;

    private void Awake()
    {
        orderIndex = transform.GetSiblingIndex();
    }

    public override void InitData(CakeData slotData, int amount) {
        base.InitData(slotData);
        imgIcon.sprite = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(slotData.id)==1 ? slotData.icons[0] : slotData.icons[1];
        imgCheck.gameObject.SetActive(false);
        totalCakeRequire = amount;
        ChangeProgress();
    }

    public bool IsThatCake(int cakeID)
    {
        return data.id == cakeID;
    }

    public void OrderDone()
    {
        imgIcon.transform.DOPunchScale(vectorPunch, .25f);
        ChangeProgress();
    }

    void ChangeProgress() {
        orderIndex = orderIndex == -1 ? orderIndex = transform.GetSiblingIndex() : orderIndex;

        currentCakeDone = ProfileManager.Instance.playerData.cakeSaveData.GetProgressOrder(orderIndex);

        txtCountCake.text = $"{currentCakeDone}/{totalCakeRequire}";

        if (totalCakeRequire <= currentCakeDone)
        {
            imgCheck.transform.DOPunchScale(vectorPunch, .25f).SetDelay(.2f);
            imgCheck.gameObject.SetActive(true);
            objCountCakeWrap.SetActive(false);
            ProfileManager.Instance.playerData.cakeSaveData.orderProgress.SetIsDone(transform.GetSiblingIndex());
        }
    }

    public bool IsDone() {
        return totalCakeRequire <= currentCakeDone;
    }
}
