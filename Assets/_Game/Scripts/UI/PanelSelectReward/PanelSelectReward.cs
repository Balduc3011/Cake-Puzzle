using AssetKits.ParticleImage.Enumerations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSelectReward : UIPanel
{
    [SerializeField] List<RewardCard> rewardCards;
    [SerializeField] Button closeBtn;
    public override void Awake()
    {
        panelType = UIPanelType.PanelSelectReward;
        base.Awake();
        closeBtn.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        if(ProfileManager.Instance.playerData.playerResourseSave.currentLevel <= 4)
        {
            rewardCards[0].ToOpenPoint();
            rewardCards[1].ToRootPoint();
            Invoke("ShowClose", 4f);
        }
        else
        {
            for (int i = 0; i < rewardCards.Count; i++)
            {
                rewardCards[i].ToHoldPoint();
            }
            closeBtn.gameObject.SetActive(false);
        }
    }

    public void OnSelectCard(int cardId)
    {
        for (int i = 0; i < rewardCards.Count; i++)
        {
            if (rewardCards[i].cardID == cardId)
                rewardCards[i].ToOpenPoint();
            else
                rewardCards[i].HideCard();
        }
        Invoke("ShowClose", 4f);
    }
    void ShowClose()
    {
        closeBtn.gameObject.SetActive(true);
    }

    void ClosePanel()
    {
        UIManager.instance.ClosePanelSelectReward();
        //if (GameManager.Instance.cakeManager.justUnlockedCake != -1 && GameManager.Instance.cakeManager.justUnlockedCake != 0)
        //{
        //    UIManager.instance.ShowPanelCakeReward();
        //}
        //if (GameManager.Instance.cakeManager.levelUp)
        //{
        //    GameManager.Instance.cakeManager.cakeShowComponent.ShowNormalCake();
        //    GameManager.Instance.cakeManager.cakeShowComponent.ShowNextToUnlockCake();
        //    GameManager.Instance.cakeManager.ClearAllCake();
        //    GameManager.Instance.cakeManager.SetOnMove(false);
        //    UIManager.instance.ShowPanelLoading();
        //}
        GameManager.Instance.cakeManager.cakeShowComponent.ShowNormalCake();
        GameManager.Instance.cakeManager.cakeShowComponent.ShowNextToUnlockCake();
        GameManager.Instance.cakeManager.ClearAllCake();
        GameManager.Instance.cakeManager.SetOnMove(false);
        UIManager.instance.ShowPanelLoading();
    }
}
