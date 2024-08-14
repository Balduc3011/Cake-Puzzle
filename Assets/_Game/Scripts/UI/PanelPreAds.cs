using AssetKits.ParticleImage;
using DG.Tweening;
using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelPreAds : UIPanel
{
    [SerializeField] CanvasGroup mainCanvasGroup;
    [SerializeField] float timeDuration;
    [SerializeField] ParticleImage rewardEffect;
    [SerializeField] Transform coinBar;

    public override void Awake()
    {
        CheckScreenObstacleBase();
    }

    // Update is called once per frame
    void OnEnable()
    {
        Sequence mainSquence = DOTween.Sequence();
        mainCanvasGroup.alpha = 0;
        mainSquence.Append(mainCanvasGroup.DOFade(1, timeDuration / 2).SetEase(Ease.InOutQuad).From(0));
      
        mainSquence.Play();
        DOVirtual.DelayedCall(2, ShowAds);
        DOVirtual.DelayedCall(3, ShowClose);
        transform.SetAsLastSibling();
        coinBar.localScale = Vector3.zero;
    }

    void ShowAds()
    {
        if (GameManager.Instance.IsHasNoAds()) return;
        if (ProfileManager.Instance.versionStatus == VersionStatus.Cheat) return;
        AdsManager.Instance.ShowInterstitial();
        //GameManager.Instance.CollectInterGold();
    }

    void ShowClose()
    {
        ProfileManager.Instance.playerData.playerResourseSave.AddMoney(ConstantValue.VAL_INTER_REWARD);
        ShowRewardEffect();
        coinBar.DOScale(1, 0.25f);
        DOVirtual.DelayedCall(1.25f, CloseInstant);
    }
    void CloseInstant()
    {
        UIManager.instance.ClosePanelPreAds();
    }

    void ShowRewardEffect()
    {
        rewardEffect.Play();
    }
}
