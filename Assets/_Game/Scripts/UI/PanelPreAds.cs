using DG.Tweening;
using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPreAds : UIPanel
{
    [SerializeField] CanvasGroup mainCanvasGroup;
    [SerializeField] float timeDuration;

    public override void Awake()
    {
        panelType = UIPanelType.PanelPreAds;
        base.Awake();
    }

    // Update is called once per frame
    void OnEnable()
    {
        Sequence mainSquence = DOTween.Sequence();
        mainCanvasGroup.alpha = 0;
        mainSquence.Append(mainCanvasGroup.DOFade(1, timeDuration / 2).SetEase(Ease.InOutQuad).From(0));
      
        mainSquence.Play();
        DOVirtual.DelayedCall(4, ShowAds);
        transform.SetAsLastSibling();
    }

    void ShowAds()
    {
        UIManager.instance.ClosePanelPreAds();
        if (GameManager.Instance.IsHasNoAds()) return;
        if (ProfileManager.Instance.versionStatus == VersionStatus.Cheat) return;
        AdsManager.Instance.ShowInterstitial();
        ProfileManager.Instance.playerData.playerResourseSave.AddMoney(5);
    }
}
