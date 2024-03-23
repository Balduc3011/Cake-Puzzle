using AssetKits.ParticleImage.Enumerations;
using DG.Tweening;
using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Button hintObj;
    [SerializeField] SheetAnimation sheetAnimation;
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
    }

    private void OnEnable()
    {
        panelWrapTrs.DOScale(1, 0.35f).From(0);
        bgCanvanGroup.DOFade(1, 0.35f).From(0);
        btnReviveCoin.interactable = ProfileManager.Instance.playerData.playerResourseSave.IsHasEnoughMoney(750);
        hintObj.gameObject.SetActive(false);
    }

    void OnClose()
    {
        panelWrapTrs.DOScale(0, 0.35f).From(1).SetEase(Ease.InOutBack);
        bgCanvanGroup.DOFade(0, 0.35f).From(1).OnComplete(ClosePanel);
    }

    void ExitPanel() {
        OnClose();
        ProfileManager.Instance.playerData.cakeSaveData.ClearAllCake();
        GameManager.Instance.cakeManager.SetOnMove(false);
        GameManager.Instance.ClearAllCake();
        //UIManager.instance.ShowPanelLoading();
        GameManager.Instance.BackToMenu();
    }

    void ClosePanel()
    {
        UIManager.instance.ClosePanelLevelComplete();
        UIManager.instance.ShowPanelLeaderBoard();
    }

    void ShowPanelHint()
    {
        hintObj.gameObject.SetActive(true);
        sheetAnimation.PlayAnim();
    }

    void ReviveADS()
    {
        if (GameManager.Instance.IsHasNoAds())
            ReviveADSSucces();
        else
            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.GameOverRevive.ToString(), ReviveADSSucces);
        //ReviveADSSucces();
        //OnClose();
    }

    void ReviveADSSucces()
    {
        UIManager.instance.OpenBlockAll();
        GameManager.Instance.cakeManager.TrashIn(GameManager.Instance.cakeManager.ClearCake);
        OnClose();
    }

    void ReviveCoin()
    {
        ProfileManager.Instance.playerData.playerResourseSave.ConsumeMoney(ConstantValue.VAL_REVICE_PRICE);
        UIManager.instance.OpenBlockAll();
        GameManager.Instance.cakeManager.TrashIn(GameManager.Instance.cakeManager.ClearCake);
        OnClose();
    }

    void OnWinClose()
    {
        OnClose();
    }

    public void ShowPanel(bool isWinGame)
    {
        objLooseGame.SetActive(!isWinGame);
        objWinGame.SetActive(isWinGame);
    }
}
