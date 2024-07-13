using AssetKits.ParticleImage;
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
    Transform moveTarget;
    //[SerializeField] GameObject border1;

    List<ItemData> rewards = new();
    ItemData toReward;
    private void Start()
    {
        cardBtn.onClick.AddListener(SelectCard);
    }

    public void ActiveBtn(bool value)
    {
        cardBtn.interactable = value;
    }

    public void ShowCardReward()
    {
        if(toReward.ItemType == ItemType.Coin)
        {
            ParticleImage rewardEffect = panelSelectReward.GetRewardEffect();
            if (rewardEffect == null) return;
            rewardEffect.transform.position = Transform.position;
            rewardEffect.texture = rewardIcon.sprite.texture;
            rewardEffect.SetBurst(0, 0, toReward.amount < 10 ? (int)(toReward.amount) : 10);
            rewardEffect.Play();
            moveTarget = panelSelectReward.GetBoosterPos(toReward.ItemType);
            if (moveTarget != null)
            {
                rewardEffect.attractorTarget = moveTarget;
            }
            else
            {
                rewardEffect.attractorTarget = panelSelectReward.itemsBag;
            }
            //rewardEffect.onLastParticleFinish = EffectMoveDone;
            panelSelectReward.SetEffectDoneMoveAct(EffectMoveDone);
        }
    }

    void EffectMoveDone()
    {
        moveTarget.DOScale(1, 0.05f);
    }

    void SelectCard()
    {
        panelSelectReward.OnSelectCard(cardID);
        cardBtn.interactable = false;
        //ShowCardReward();
        ShowCard();
    }

    public void ShowCard()
    {
        Transform.DORotate(Vector3.up * 180, 1.4f).SetEase(Ease.InOutQuart);
        cardLight.DOFade(1, 0.15f).SetDelay(0.7f).OnComplete(() =>
        {
            bg.SetActive(false);
            main.SetActive(true);

        });
        cardLight.DOFade(0, 1f).SetDelay(0.7f + 0.25f).OnComplete(ShowCardReward);
    }

    public void HideCard()
    {
        Transform.DOScale(0, 0.25f).From(1).SetEase(Ease.InBack);
        cardBtn.interactable = false;
    }

    public void ToRootPoint()
    {
        Transform.position = rootPoint.position;
    }

    public void ToHoldPoint()
    {
        Transform.DOScale(1, 0.5f).From(0);
        Transform.DOMove(openPoint.position, 0.25f).SetEase(Ease.OutBack);
        Transform.DOMove(holdPoint.position, 0.25f).SetEase(Ease.InOutQuad).SetDelay(1).OnComplete(() =>
        {
            cardBtn.interactable = true;
            //bg.SetActive(false);
            //main.SetActive(true);

        }); ;
        //Transform.DORotate(Vector3.up * 180, 1.4f).SetEase(Ease.InOutQuart).SetDelay(1.5f);
        //cardLight.DOFade(1, 0.15f).SetDelay(0.7f + 1.5f).OnComplete(() =>
        //{
        //    cardBtn.interactable = true;
        //    bg.SetActive(false);
        //    main.SetActive(true);

        //});
        //cardLight.DOFade(0, 1f).SetDelay(0.7f + 1.5f + 0.25f);
    }

    public void ToOpenPoint()
    {
        cardBtn.interactable = false;
        Transform.localScale = Vector3.one;
        Transform.DOMove(openPoint.position, 0.35f).SetEase(Ease.InBack);
    }

    public void SingleOpen()
    {
        Transform.localScale = Vector3.one;
        Transform.DOMove(openPoint.position, 0.35f).SetEase(Ease.InBack);
        Transform.DORotate(Vector3.up * 180, 1.4f).SetEase(Ease.InOutQuart).SetDelay(0.5f);
        cardLight.DOFade(1, 0.15f).SetDelay(0.7f + 0.5f).OnComplete(() =>
        {
            cardBtn.interactable = true;
            bg.SetActive(false);
            main.SetActive(true);

        });
        cardLight.DOFade(0, 1f).SetDelay(0.7f + 0.5f + 0.25f).OnComplete(ShowCardReward);
    }
    public void ToHoldEx()
    {
        Transform.DOMove(holdPoint.position, 0.25f).SetEase(Ease.InOutQuad);
    }
    public void ToHoldOpen()
    {
        Transform.DOScale(1, 0.5f).From(0);
    }


    private void OnEnable()
    {
        Init();
        cardLight.alpha = 0;
        bg.SetActive(true);
        main.SetActive(false);
        cardBtn.interactable = false;
        Transform.position = rootPoint.position;
        Transform.eulerAngles = Vector3.zero;
    }

    public void Init()
    {
        rewards = GameManager.Instance.rewardItems;
        if (rewards == null) return;
        if (rewards.Count == 0) return;
        if(cardID < rewards.Count)
        {
            toReward = rewards[cardID];
            //rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(toReward.ItemType);
            rewardAmountTxt.text = toReward.amount.ToString();
            //border1.SetActive(toReward.ItemType == ItemType.Cake);
            if (toReward.ItemType != ItemType.Cake)
                rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(toReward.ItemType);
            else
                rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetCakeSprite(toReward.subId);
        }
    }
}
