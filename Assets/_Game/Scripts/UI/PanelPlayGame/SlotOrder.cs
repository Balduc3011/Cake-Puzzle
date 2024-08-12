using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlotOrder : SlotBase<CakeData>
{
    [SerializeField] Image imgIcon;
    [SerializeField] Image imgCheck;
    [SerializeField] Vector3 vectorPunch = new Vector3(.5f, .5f, .5f);

    public override void InitData(CakeData slotData) {
        base.InitData(slotData);
        imgIcon.sprite = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(slotData.id)==1 ? slotData.icons[0] : slotData.icons[1];
        imgCheck.gameObject.SetActive(false);
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
