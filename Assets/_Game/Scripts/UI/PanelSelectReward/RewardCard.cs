using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    public int cardID;
    [SerializeField] Button cardBtn;
    [SerializeField] Transform Transform;
    [SerializeField] Transform rootPoint;
    [SerializeField] Transform openPoint;
    [SerializeField] Transform holdPoint;
    [SerializeField] PanelSelectReward panelSelectReward;
    [SerializeField] Image rewardIcon;
    [SerializeField] TextMeshProUGUI rewardAmountTxt;
    [SerializeField] GameObject main;
    [SerializeField] GameObject bg;
    [SerializeField] CanvasGroup cardLight;

    List<ItemData> rewards = new();
    ItemData toReward;
    private void Start()
    {
        cardBtn.onClick.AddListener(SelectCard);
    }

    void SelectCard()
    {
        panelSelectReward.OnSelectCard(cardID);
        cardBtn.interactable = false;
    }
    public void HideCard()
    {
        Transform.DOScale(0, 0.5f).From(1).SetEase(Ease.InBack);
        cardBtn.interactable = false;
    }

    public void ToRootPoint()
    {
        Transform.position = rootPoint.position;
    }

    public void ToHoldPoint()
    {
        Transform.DOScale(1, 0.5f).From(0);
        Transform.DOMove(openPoint.position, 0.75f).SetEase(Ease.OutBack);
        Transform.DOMove(holdPoint.position, 0.75f).SetEase(Ease.InOutQuad).SetDelay(1);
    }

    public void ToOpenPoint()
    {
        Transform.localScale = Vector3.one;
        Transform.DOMove(openPoint.position, 0.75f).SetEase(Ease.InBack);
        Transform.DORotate(Vector3.up * 180, 1.5f).SetDelay(1).SetEase(Ease.InOutQuart);
        cardLight.DOFade(1, 0.15f).SetDelay(1.75f).OnComplete(() =>
        {
            bg.SetActive(false);
            main.SetActive(true);
        });
        
        cardLight.DOFade(0, 1f).SetDelay(3f);
    }

    

    private void OnEnable()
    {
        rewards = GameManager.Instance.rewardItems;
        if (rewards == null) return;
        if (rewards.Count == 0) return;
        toReward = rewards[0];
        rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(toReward.ItemType);
        rewardAmountTxt.text = toReward.amount.ToString();
        cardLight.alpha = 0;
        bg.SetActive(true);
        main.SetActive(false);
        cardBtn.interactable = true;
        Transform.position = rootPoint.position;
        Transform.eulerAngles = Vector3.zero;
    }
}
