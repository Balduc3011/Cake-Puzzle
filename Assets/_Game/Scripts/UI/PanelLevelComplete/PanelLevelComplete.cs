using AssetKits.ParticleImage.Enumerations;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelComplete : UIPanel
{
    [SerializeField] Button btnReviveCoin;
    [SerializeField] Button btnReviveAds;
    [SerializeField] Button winGameCloseBtn;
    [SerializeField] Button btnExit;
    [SerializeField] Transform panelWrapTrs;
    [SerializeField] CanvasGroup bgCanvanGroup;
    [SerializeField] GameObject objWinGame;
    [SerializeField] GameObject objLooseGame;
    [SerializeField] TextMeshProUGUI revivePrivceTxt;
    [SerializeField] Button hintObj;
    [SerializeField] SheetAnimation sheetAnimation;
    [SerializeField] SheetAnimation loseSheetAnimation;
    [SerializeField] SheetAnimation winSheetAnimation;

    public override void Awake()
    {
        panelType = UIPanelType.PanelLevelComplete;
        base.Awake();
    }
    void Start()
    {
        btnReviveCoin.onClick.AddListener(ReviveCoin);
        btnReviveAds.onClick.AddListener(ReviveADS);
        btnExit.onClick.AddListener(ShowPanelHint);
        hintObj.onClick.AddListener(ExitPanel);
        winGameCloseBtn.onClick.AddListener(ExitPanel);
        revivePrivceTxt.text = ConstantValue.VAL_REVIVE_COIN.ToString();
    }

    private void OnEnable()
    {
        objLooseGame.SetActive(true);
        panelWrapTrs.DOScale(1, 0.35f).From(0);
        bgCanvanGroup.DOFade(1, 0.35f).From(0);
        btnReviveCoin.interactable = ProfileManager.Instance.playerData.playerResourseSave.IsHasEnoughMoney(ConstantValue.VAL_REVIVE_COIN);
        hintObj.gameObject.SetActive(false);
        transform.SetAsLastSibling();
    }

    void OnClose()
    {
        panelWrapTrs.DOScale(0, 0.35f).From(1).SetEase(Ease.InOutBack);
        bgCanvanGroup.DOFade(0, 0.35f).From(1).OnComplete(ClosePanel);
    }

    void ExitPanel() {
        OnClose();
        GameManager.Instance.ShowInter();
        ProfileManager.Instance.playerData.cakeSaveData.ClearAllCake();
        GameManager.Instance.cakeManager.SetOnMove(false);
        GameManager.Instance.ClearAllCake();
        GameManager.Instance.BackToMenu();
        UIManager.instance.ShowPanelLoading();
    }

    void ClosePanel()
    {
        UIManager.instance.ClosePanelLevelComplete();
        //UIManager.instance.ShowPanelLeaderBoard();
    }

    void ShowPanelHint()
    {
        objLooseGame.SetActive(false);
        objWinGame.SetActive(false);
        hintObj.gameObject.SetActive(true);
        sheetAnimation.PlayAnim();
    }

    void ReviveADS()
    {
        //if (GameManager.Instance.IsHasNoAds())
        //    ReviveADSSucces();
        //else
        //    AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.GameOverRevive.ToString(), ReviveADSSucces);

        GameManager.Instance.ShowRewardVideo(WatchVideoRewardType.GameOverRevive, ReviveADSSucces);
    }

    void ReviveADSSucces()
    {
        GameManager.Instance.itemManager.UsingItem(ItemType.Revive);
        EventManager.TriggerEvent(EventName.OnUsingRevive.ToString());
        OnClose();
    }

    void ReviveCoin()
    {
        ProfileManager.Instance.playerData.playerResourseSave.ConsumeMoney(ConstantValue.VAL_REVIVE_COIN);
        //UIManager.instance.OpenBlockAll();
        GameManager.Instance.itemManager.UsingItem(ItemType.Revive);
        EventManager.TriggerEvent(EventName.OnUsingRevive.ToString());
        //GameManager.Instance.cakeManager.TrashIn(GameManager.Instance.cakeManager.ClearCake);
        OnClose();
    }

    public void ShowPanel(bool isWinGame)
    {
        objLooseGame.SetActive(!isWinGame);
        objWinGame.SetActive(isWinGame);
        if (isWinGame) winSheetAnimation.PlayAnim();
        //else loseSheetAnimation.PlayAnim();
    }
}
