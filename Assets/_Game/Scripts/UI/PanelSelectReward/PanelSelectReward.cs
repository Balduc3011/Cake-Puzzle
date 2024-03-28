using AssetKits.ParticleImage;
using DG.Tweening;
using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSelectReward : UIPanel
{
    [SerializeField] List<RewardCard> rewardCards;
    [SerializeField] Button panelCloseBtn;
    [SerializeField] Button closeBtn;
    [SerializeField] Button extraAdsBtn;
    [SerializeField] int selectedCardId;
    List<ItemData> rewards = new();
    [SerializeField] Transform itemsBag;
    [SerializeField] List<ParticleImage> rewardEffect;
    public override void Awake()
    {
        panelType = UIPanelType.PanelSelectReward;
        base.Awake();
        panelCloseBtn.onClick.AddListener(ClosePanel);
        closeBtn.onClick.AddListener(ClosePanel);
        extraAdsBtn.onClick.AddListener(GetExtraByAds);
    }

    private void OnEnable()
    {
        itemsBag.localScale = Vector3.zero;
        transform.SetAsLastSibling();
        selectedCardId = -1;
        rewards = GameManager.Instance.rewardItems;
        if (ProfileManager.Instance.playerData.playerResourseSave.currentLevel < 4)
        {
            rewardCards[0].SingleOpen();
            rewardCards[1].ToRootPoint();
            GameManager.Instance.AddItem(rewards[0]);
            Invoke("ShowClose", 3f);
        }
        else
        {
            for (int i = 0; i < rewardCards.Count; i++)
            {
                rewardCards[i].ToHoldPoint();
            }
        }
        panelCloseBtn.gameObject.SetActive(false);
        closeBtn.transform.localScale = Vector3.zero;
        extraAdsBtn.transform.localScale = Vector3.zero;
    }

    public void OnSelectCard(int cardId)
    {
        selectedCardId = cardId;
        for (int i = 0; i < rewardCards.Count; i++)
        {
            if (rewardCards[i].cardID == cardId)
                rewardCards[i].HideCard();
            else
                rewardCards[i].ToOpenPoint();
        }
        Invoke("ShowClose", 1.5f);
        GameManager.Instance.AddItem(rewards[cardId]);
    }
    void ShowClose()
    {
        
        if (ProfileManager.Instance.playerData.playerResourseSave.currentLevel < 4)
        {
            panelCloseBtn.gameObject.SetActive(true);
        }
        else
        {
            closeBtn.transform.DOScale(1, 0.25f).SetEase(Ease.OutBack);
            extraAdsBtn.transform.DOScale(1, 0.25f).SetEase(Ease.OutBack);
        }
    }

    void ShowClosePanel()
    {
        panelCloseBtn.gameObject.SetActive(true);
    }

    void ClosePanel()
    {
        UIManager.instance.ClosePanelSelectReward();
        GameManager.Instance.cakeManager.cakeShowComponent.ShowNormalCake();
        GameManager.Instance.cakeManager.cakeShowComponent.ShowNextToUnlockCake();
        GameManager.Instance.cakeManager.ClearAllCake();
        GameManager.Instance.cakeManager.SetOnMove(false);
        UIManager.instance.ShowPanelLoading();
    }

    void GetExtraByAds()
    {
        if (GameManager.Instance.IsHasNoAds())
            OnGetExtraReward();
        else
            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.GetExtraCard.ToString(), OnGetExtraReward);
    }

    void OnGetExtraReward()
    {
        Invoke("ShowClosePanel", 3f);
        for (int i = 0; i < rewardCards.Count; i++)
        {
            if (rewardCards[i].cardID == selectedCardId)
            {
                //rewardCards[i].ToHoldEx();
            }
            else
            {
                rewardCards[i].ShowCardReward();
                rewardCards[i].HideCard();
                GameManager.Instance.AddItem(rewards[i]);
            }
                
        }
        closeBtn.transform.DOScale(0, 0.25f).SetEase(Ease.InBack);
        extraAdsBtn.transform.DOScale(0, 0.25f).SetEase(Ease.InBack);
    }

    public void ItemToBag()
    {
        itemsBag.DOScale(0.9f, 0.05f);
        itemsBag.DOScale(1f, 0.05f).SetEase(Ease.OutBack).SetDelay(0.05f);
    }

    public ParticleImage GetRewardEffect()
    {
        for (int i = 0; i < rewardEffect.Count; i++)
        {
            if (!rewardEffect[i].isPlaying)
                return rewardEffect[i];
        }
        return null;
    }
}
