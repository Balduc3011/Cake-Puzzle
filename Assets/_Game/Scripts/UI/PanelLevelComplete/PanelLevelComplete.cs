using AssetKits.ParticleImage.Enumerations;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelComplete : UIPanel
{
    [SerializeField] Button loseGameContinueBtn;
    [SerializeField] Button loseGameCloseBtn;
    [SerializeField] Button winGameCloseBtn;
    [SerializeField] Transform panelWrapTrs;
    [SerializeField] CanvasGroup bgCanvanGroup;
    public override void Awake()
    {
        panelType = UIPanelType.PanelLevelComplete;
        base.Awake();
    }
    void Start()
    {
        loseGameCloseBtn.onClick.AddListener(OnLoseClose);
        loseGameContinueBtn.onClick.AddListener(OnContinueAdsClick);
    }

    private void OnEnable()
    {
        panelWrapTrs.DOScale(1, 0.35f).From(0);
        bgCanvanGroup.DOFade(1, 0.35f).From(0);
    }

    void OnClose()
    {
        panelWrapTrs.DOScale(0, 0.35f).From(1);
        bgCanvanGroup.DOFade(0, 0.35f).From(1).OnComplete(ClosePanel);
    }

    void ClosePanel()
    {
        UIManager.instance.ClosePanelLevelComplete();
    }

    void OnContinueAdsClick()
    {
        OnClose();
    }

    void OnLoseClose()
    {
        OnClose();
    }

    void OnWinClose()
    {
        OnClose();
    }
}
