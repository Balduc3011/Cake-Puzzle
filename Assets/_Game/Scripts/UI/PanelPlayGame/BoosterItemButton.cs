using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoosterItemButton : MonoBehaviour
{
    public ItemType boosterType;
    [SerializeField] Image itemBarIconImg;
    [SerializeField] TextMeshProUGUI itemAmountTxt;
    [SerializeField] Button btnChoose;
    [SerializeField] GameObject amountBar;
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
            amountBar.SetActive(true);
        }
        else
        {
            //itemBarIconImg.gameObject.SetActive(false);
            if (itemAmountTxt != null)
                itemAmountTxt.text = ConstantValue.STR_BLANK;
            addMoreAlert.SetActive(true);
            amountBar.SetActive(false);
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
                    UIManager.instance.ShowPanelQuickIAP(OfferID.pack_hammer);
                    break;
                case ItemType.ReRoll:
                    UIManager.instance.ShowPanelQuickIAP(OfferID.pack_reroll);
                    break;
                case ItemType.FillUp:
                    UIManager.instance.ShowPanelQuickIAP(OfferID.pack_fillup);
                    break;
                default:
                    break;
            }
        }
    }
}
