using DG.Tweening;
using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPreAds : UIPanel
{
    [SerializeField] Transform txtTransform;
    [SerializeField] Transform imgTransform;
    [SerializeField] CanvasGroup txtCanvasGroup;
    [SerializeField] CanvasGroup imgCanvasGroup;
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
        mainSquence.Append(mainCanvasGroup.DOFade(1, timeDuration / 2).SetEase(Ease.InOutQuad).From(0));
        mainSquence.Append(txtTransform.DOScale(1, timeDuration).From(0).SetEase(Ease.OutBack));
        mainSquence.Join(txtCanvasGroup.DOFade(1, timeDuration * 2).From(0).SetEase(Ease.InOutQuad));
        mainSquence.Join(imgTransform.DOScale(1, timeDuration).From(0).SetEase(Ease.OutBack).SetDelay(.2f));
        mainSquence.Join(imgTransform.DOMoveY(transform.position.y, timeDuration).From(transform.position.y - 100f).SetEase(Ease.OutBack));
        mainSquence.Join(imgCanvasGroup.DOFade(1, timeDuration * 2).From(0).SetEase(Ease.InOutQuad));
        mainSquence.Play();
        mainSquence.OnComplete(ShowAds);
    }

    void ShowAds()
    {
        UIManager.instance.ClosePanelPreAds();
        AdsManager.Instance.ShowInterstitial();
    }
}