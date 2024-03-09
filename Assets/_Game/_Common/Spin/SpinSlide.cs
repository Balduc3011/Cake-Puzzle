using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinSlide : MonoBehaviour
{
    SpinItemData spinItemData;
    public int id;
    [SerializeField] Image itemIconImg;
    [SerializeField] TextMeshProUGUI itemAmountTxt;
    [SerializeField] GameObject spinRewardObj;
    public void Init(int id, SpinItemData spinItemData)
    {
        this.id = id;
        this.spinItemData = spinItemData;
        //itemAmountTxt.text = spinItemData.amount.ToString();
        itemAmountTxt.text = spinItemData.itemData.amount.ToString();
        itemIconImg.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(spinItemData.itemData.ItemType);
    }

    public void OnReward(bool atv)
    {
        spinRewardObj.SetActive(atv);
    }
}
