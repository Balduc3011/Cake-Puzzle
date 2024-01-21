using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoosterItemButton : MonoBehaviour
{
    [SerializeField] ItemType boosterType;
    [SerializeField] Image itemBarIconImg;
    [SerializeField] TextMeshProUGUI itemAmountTxt;

    public void UpdateStatus()
    {
        int itemAmount = (int)GameManager.Instance.GetItemAmount(boosterType);
        if(itemAmount > 0 )
        {
            itemBarIconImg.gameObject.SetActive(true);
            itemAmountTxt.text = itemAmount.ToString();
        }
        else
        {
            itemBarIconImg.gameObject.SetActive(false);
            itemAmountTxt.text = ConstantValue.STR_BLANK;
        }
    }
}
