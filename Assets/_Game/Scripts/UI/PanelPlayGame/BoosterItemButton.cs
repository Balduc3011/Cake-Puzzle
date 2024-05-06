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
    [SerializeField] GameObject addMoreAlert;
    UnityAction CallBack;
    private void Start()
    {
        UpdateStatus();
        EventManager.AddListener(EventName.AddItem.ToString(), UpdateStatus);
        btnChoose.onClick.AddListener(ButtonOnClick);
    }
    public void SetActionCallBack(UnityAction actionCalback) {
        CallBack = actionCalback;
    }
    public void UpdateStatus()
    {
        int itemAmount = (int)GameManager.Instance.GetItemAmount(boosterType);
        if(itemAmount > 0 )
        {
            //itemBarIconImg.gameObject.SetActive(true);
            if (itemAmountTxt != null)
                itemAmountTxt.text = itemAmount.ToString();
            addMoreAlert.SetActive(false);
        }
        else
        {
            //itemBarIconImg.gameObject.SetActive(false);
            if (itemAmountTxt != null)
                itemAmountTxt.text = ConstantValue.STR_BLANK;
            addMoreAlert.SetActive(true);
        }
    }

    void ButtonOnClick()
    {
        int itemAmount = (int)GameManager.Instance.GetItemAmount(boosterType);
        if (itemAmount > 0)
        {
            CallBack();
            GameManager.Instance.questManager.AddProgress(QuestType.UseBooster, 1);
        }
        else
        {
            switch (boosterType)
            {
                case ItemType.Hammer:
                    UIManager.instance.ShowPanelQuickIAP(OfferID.PackHammer);
                    break;
                case ItemType.ReRoll:
                    UIManager.instance.ShowPanelQuickIAP(OfferID.PackReRoll);
                    break;
                case ItemType.FillUp:
                    UIManager.instance.ShowPanelQuickIAP(OfferID.PackFillUp);
                    break;
                default:
                    break;
            }
        }
    }
}
