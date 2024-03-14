using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAPPack : MonoBehaviour
{
    public PackageId packageId;
    public Button buyBtn;
    [SerializeField] TextMeshProUGUI priceTxt;

    public List<ItemData> rewardItems = new();
    public List<ShopItemReward> rewards = new();
    ShopPack shopPack;
    public virtual void OnEnable()
    {
        shopPack = ProfileManager.Instance.dataConfig.shopDataConfig.GetShopPack(packageId);
        if(shopPack !=  null )
        {
            rewardItems = shopPack.rewards;
        }
        for (int i = 0; i < rewards.Count; i++)
        {
            if (rewards[i].itemIcon != null)
                rewards[i].itemIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(rewardItems[i].ItemType);
            if(rewards[i].amount != null)
                rewards[i].amount.text = rewardItems[i].amount.ToString();
        }
    }

    private void Start()
    {
        buyBtn.onClick.AddListener(OnBuyPack);
        priceTxt.text = "$9.99";
    }

    void OnBuyPack()
    {
        OnBuyPackSuccess();
    }

    void OnBuyPackSuccess()
    {
        GameManager.Instance.OnBuyPackSuccess(packageId);
        UIManager.instance.ShowPanelItemsReward();
    }
}

[System.Serializable]
public class ShopItemReward
{
    public Image itemIcon;
    public TextMeshProUGUI amount;
}
