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
    
    public void Init(ItemData itemData)
    {
        titleTxt.text = itemData.ItemType.ToString();
        amountTxt.text = itemData.amount.ToString();
        if(itemData.ItemType != ItemType.Cake)
            iconImg.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(itemData.ItemType);
        else 
            iconImg.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetCakeSprite(itemData.subId);
        if (Transform == null) Transform = transform;
        contentTransform.DOScale(1, 0.2f).From(2);
        canvasGroup.DOFade(1, 0.15f).From(0);
    }
}
