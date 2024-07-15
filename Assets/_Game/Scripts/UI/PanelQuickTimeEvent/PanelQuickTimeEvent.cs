using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelQuickTimeEvent : UIPanel
{
    //[SerializeField] Transform txtTransform;
    //[SerializeField] Transform imgTransform;
    //[SerializeField] CanvasGroup txtCanvasGroup;
    //[SerializeField] CanvasGroup imgCanvasGroup;
    [SerializeField] CanvasGroup mainCanvasGroup;

    [SerializeField] float timeDuration;
    public override void Awake()
    {
        panelType = UIPanelType.PanelQuickTimeEvent;
        base.Awake();
    }

    private void OnEnable()
    {
        Sequence mainSquence = DOTween.Sequence();
        mainCanvasGroup.alpha = 0;
        mainSquence.Append( mainCanvasGroup.DOFade(1, timeDuration/2).SetEase(Ease.InOutQuad));
        //mainSquence.Append(txtTransform.DOScale(1, timeDuration).From(0).SetEase(Ease.OutBack));
        //mainSquence.Join(txtCanvasGroup.DOFade(1, timeDuration * 2).From(0).SetEase(Ease.InOutQuad));
        //mainSquence.Join(imgTransform.DOScale(1, timeDuration).From(0).SetEase(Ease.OutBack).SetDelay(.2f));
        //mainSquence.Join(imgTransform.DOMoveY(transform.position.y, timeDuration).From(transform.position.y - 100f).SetEase(Ease.OutBack));
        //mainSquence.Join(imgCanvasGroup.DOFade(1, timeDuration * 2).From(0).SetEase(Ease.InOutQuad));
        mainSquence.Play();
        mainSquence.OnComplete(AnimationDone);
        transform.SetAsLastSibling();
    }

    void AnimationDone() {
        //imgCanvasGroup.DOFade(0, timeDuration * 2).SetEase(Ease.InOutQuad);
        //txtCanvasGroup.DOFade(0, timeDuration * 2).SetEase(Ease.InOutQuad);
        mainCanvasGroup.DOFade(0, timeDuration * 2).SetEase(Ease.InOutQuad).SetDelay(timeDuration).OnComplete(UIManager.instance.ClosePanelQuickTimeEvent);
        UIManager.instance.panelTotal.ShowQuickTimeEvent();
    }
}
