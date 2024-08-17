using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SlotOrder : SlotBase<CakeData>
{
    [SerializeField] Image imgIcon;
    [SerializeField] Image imgCheck;
    [SerializeField] TextMeshProUGUI txtCountCake;
    [SerializeField] Vector3 vectorPunch = new Vector3(.5f, .5f, .5f);
    int amountRequire;

    public override void InitData(CakeData slotData, int amount) {
        base.InitData(slotData);
        imgIcon.sprite = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(slotData.id)==1 ? slotData.icons[0] : slotData.icons[1];
        imgCheck.gameObject.SetActive(false);
        amountRequire = amount;
        txtCountCake.text = amount.ToString();
    }

    public bool IsThatCake(int cakeID)
    {
        return data.id == cakeID;
    }

    public void OrderDone()
    {
        imgIcon.transform.DOPunchScale(vectorPunch, .25f);
        imgCheck.transform.DOPunchScale(vectorPunch, .25f).SetDelay(.2f);
        imgCheck.gameObject.SetActive(true);
    }

    public bool IsDone() {
        return imgCheck.gameObject.activeSelf;
    }
}
