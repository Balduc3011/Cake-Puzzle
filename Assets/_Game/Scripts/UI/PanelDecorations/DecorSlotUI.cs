using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class DecorSlotUI : MonoBehaviour
{
    DecorationType decorationType;
    DecorationData decorationData;
    [SerializeField] Button buyBtn;
    [SerializeField] Button useBtn;
    [SerializeField] GameObject usingObj;
    [SerializeField] TextMeshProUGUI priceTxt;
    [SerializeField] Image decorIconImg;

    private void Start()
    {
        buyBtn.onClick.AddListener(OnBuy);
        useBtn.onClick.AddListener(OnUse);
    }
    public void InitSlot(DecorationType decorationType, DecorationData decorationData)
    {
        this.decorationType = decorationType;
        this.decorationData = decorationData;
        decorIconImg.sprite = decorationData.icon;
        if (GameManager.Instance.decorationManager.IsOwned(decorationType, decorationData.id))
        {
            buyBtn.gameObject.SetActive(false);
            SetUse(GameManager.Instance.decorationManager.IsInUse(decorationType, decorationData.id));
        }
        else
        {
            useBtn.gameObject.SetActive(false);
            usingObj.SetActive(false);
            buyBtn.gameObject.SetActive(true);
            priceTxt.text = decorationData.price.ToString();
            buyBtn.interactable = ProfileManager.Instance.playerData.playerResourseSave.IsHasEnoughMoney(decorationData.price);
        }
    }

    void OnBuy()
    {
        GameManager.Instance.decorationManager.BuyDecor(decorationType, decorationData.id);
    }

    void OnUse()
    {
        GameManager.Instance.decorationManager.UseDecor(decorationType, decorationData.id);
    }

    void SetUse(bool use)
    {
        useBtn.gameObject.SetActive(!use);
        usingObj.SetActive(use);
    }

}
