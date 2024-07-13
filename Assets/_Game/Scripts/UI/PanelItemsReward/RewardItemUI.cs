using AssetKits.ParticleImage;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemUI : MonoBehaviour
{
    Transform Transform;
    [SerializeField] Transform contentTransform;
    [SerializeField] Image iconImg;
    [SerializeField] TextMeshProUGUI titleTxt;
    [SerializeField] TextMeshProUGUI amountTxt;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] ParticleImage rewardEffect;
    PanelItemsReward panelItemsReward;

    public void Init(ItemData itemData, PanelItemsReward panelItemsReward = null)
    {
        this.panelItemsReward = panelItemsReward;
        titleTxt.text = itemData.ItemType.ToString();
        amountTxt.text = itemData.amount.ToString();
        if(itemData.ItemType != ItemType.Cake)
            iconImg.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(itemData.ItemType);
        else 
            iconImg.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetCakeSprite(itemData.subId);
        if (Transform == null) Transform = transform;
        contentTransform.DOScale(1, 0.25f).From(2);
        canvasGroup.DOFade(1, 0.15f).From(0);
        if (itemData.ItemType != ItemType.NoAds) {
            if (itemData.ItemType == ItemType.Coin && panelItemsReward != null)
            {
                rewardEffect.attractorTarget = panelItemsReward.coinBar;
                rewardEffect.SetBurst(0, 0, 10);
                rewardEffect.texture = iconImg.sprite.texture;
                rewardEffect.Play();
            }
            //else
            //{
            //    rewardEffect.attractorTarget = panelItemsReward.bagBar;
            //    rewardEffect.SetBurst(0, 0, itemData.amount < 5 ? (int)(itemData.amount) : 5);
            //}
        }
    }
}
