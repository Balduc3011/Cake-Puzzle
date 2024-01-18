using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemUI : MonoBehaviour
{
    Transform Transform;
    [SerializeField] Image iconImg;
    [SerializeField] TextMeshProUGUI titleTxt;
    [SerializeField] TextMeshProUGUI amountTxt;
    
    public void Init(ItemData itemData)
    {
        titleTxt.text = itemData.ItemType.ToString();
        amountTxt.text = itemData.amount.ToString();
        iconImg.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetSprite(itemData.ItemType);
        if (Transform == null) Transform = transform;
        Transform.localScale = Vector3.zero;
        Transform.DOScale(1, 0.1f);
    }
}
