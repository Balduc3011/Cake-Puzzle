using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoosterItemButton : MonoBehaviour
{
    [SerializeField] ItemType boosterType;
    [SerializeField] Image itemBarIconImg;
    [SerializeField] TextMeshProUGUI itemAmountTxt;
    [SerializeField] Button btnChoose;
    private void Start()
    {
        UpdateStatus();
    }
    public void SetActionCallBack(UnityAction actionCalback) {
        btnChoose.onClick.AddListener(actionCalback);
    }
    public void UpdateStatus()
    {
        int itemAmount = (int)GameManager.Instance.GetItemAmount(boosterType);
        if(itemAmount > 0 )
        {
            itemBarIconImg.gameObject.SetActive(true);
            if (itemAmountTxt != null)
                itemAmountTxt.text = itemAmount.ToString();
        }
        else
        {
            itemBarIconImg.gameObject.SetActive(false);
            if (itemAmountTxt != null)
                itemAmountTxt.text = ConstantValue.STR_BLANK;
        }
    }
}
