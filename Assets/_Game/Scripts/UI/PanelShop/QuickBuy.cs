using SDK;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickBuy : MonoBehaviour
{
    public OfferID packageId;
    public List<ItemData> rewardItems = new();
    public List<ShopItemReward> rewards = new();
    public Button buyBtn;
    public Button adsBtn;
    ShopPack shopPack;
    [SerializeField] bool dontNeedData;
    [SerializeField] TextMeshProUGUI priceTxt;
    public virtual void OnEnable()
    {
        if(!dontNeedData)
        {
            shopPack = ProfileManager.Instance.dataConfig.shopDataConfig.GetShopPack(packageId);
            if (shopPack != null)
            {
                rewardItems = shopPack.rewards;
            }
        }
        for (int i = 0; i < rewards.Count; i++)
        {
            if (rewards[i].itemIcon != null)
                rewards[i].itemIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(rewardItems[i].ItemType);
            if (rewards[i].amount != null)
                rewards[i].amount.text = rewardItems[i].amount.ToString();
        }
    }

    private void Start()
    {
        buyBtn.onClick.AddListener(OnBuyPack);
        adsBtn.onClick.AddListener(OnWatchAds);
        priceTxt.text = "50";
    }

    void OnBuyPack()
    {
        if (ProfileManager.Instance.playerData.playerResourseSave.IsHasEnoughMoney(50))
        {
            ProfileManager.Instance.playerData.playerResourseSave.ConsumeMoney(50);
            OnBuyPackSuccess();
        }
        else
        {
            UIManager.instance.ClosePanelQuickIAP();
            UIManager.instance.ShowPanelQuickIAP(OfferID.pack_money1);
        }
    }
    void OnWatchAds()
    {
        //if(GameManager.Instance.IsHasNoAds())
        //{
        //    OnBuyPackSuccess();
        //}
        //else
        //{
        //    AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.GetFreeBooster.ToString(), OnBuyPackSuccess);
        //}
        GameManager.Instance.ShowRewardVideo(WatchVideoRewardType.GetFreeBooster, OnBuyPackSuccess);
    }

    void OnBuyPackSuccess()
    {
        GameManager.Instance.GetItemRewards(rewardItems);
        UIManager.instance.ClosePanelQuickIAP();
        UIManager.instance.ShowPanelItemsReward();
    }
}
