using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotOrder : SlotBase<CakeData>
{
    [SerializeField] Image imgIcon;
    [SerializeField] Image imgCheck;

    public override void InitData(CakeData slotData) {
        base.InitData(slotData);
        imgIcon.sprite = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(slotData.id)==1 ? slotData.icons[0] : slotData.icons[1];
        imgCheck.gameObject.SetActive(false);
    }
}
