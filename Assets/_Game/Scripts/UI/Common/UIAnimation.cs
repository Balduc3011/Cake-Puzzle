using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

namespace UIAnimation
{
    public static class UIAnimationController {
        public static Vector3 vector10 = Vector3.one;
        public static Vector3 vector00 = Vector3.zero;
        public static Vector3 vector11 = new Vector3(1.1f, 1.1f, 1.1f);
        public static Vector3 vector08 = new Vector3(.8f, .8f, .8f);
        public static Vector3 vector09 = new Vector3(.9f, .9f, .9f);

        public static Sequence BasicButton(Transform trs, float duration = .25f, UnityAction actionCallBack = null) {
            trs.localScale = vector10;
            Sequence mainSequence = DOTween.Sequence();
            mainSequence.Append(trs.DOScale(vector09, duration / 3));
            mainSequence.Append(trs.DOScale(vector11, duration / 3));
            mainSequence.Append(trs.DOScale(vector10, duration / 3));
            mainSequence.OnComplete(() =>
            {
                if (actionCallBack != null)
                {
                    actionCallBack();
                }
            });

            return mainSequence;
        }

        public static Sequence SlotAnimation(Transform trs, float duration = .25f, UnityAction actionCallBack = null) {
            trs.localScale = vector08;
            Sequence mainSequence = DOTween.Sequence();
            mainSequence.Append( trs.DOScale(vector10, duration));
            mainSequence.OnComplete(() =>
            {
                if (actionCallBack != null)
                {
                    actionCallBack();
                }
            });

            return mainSequence;
        }

        public static Sequence GroupAlphaAnimation(CanvasGroup cGroup, float defaultAlpha=0, float alphaTo =1, float duration = .25f, UnityAction actionCallBack = null) {
            cGroup.alpha = defaultAlpha;
            Sequence mainSequence = DOTween.Sequence();
            mainSequence.Append(cGroup.DOFade(alphaTo, duration));
            mainSequence.OnComplete(() =>
            {
                if (actionCallBack != null)
                {
                    actionCallBack();
                }
            });

            return mainSequence;
        }

        public static Sequence ShowNotice(Transform trs, float duration, UnityAction actionCallBack = null)
        {
            trs.localScale = vector00;
            Sequence mainSequence = DOTween.Sequence();
            mainSequence.Append(trs.DOScale(vector11, duration / 3));
            mainSequence.Append(trs.DOScale(vector09, duration / 3));
            mainSequence.Append(trs.DOScale(vector10, duration / 3));
            mainSequence.OnComplete(() =>
            {
                if (actionCallBack != null)
                {
                    actionCallBack();
                }
            });

            return mainSequence;
        }

        public static Sequence ShowPopup(Transform trs, float duration, UnityAction actionCallBack = null)
        {
            trs.localScale = vector00;
            Sequence mainSequence = DOTween.Sequence();
            mainSequence.Append(trs.DOScale(vector11, duration / 3));
            mainSequence.Append(trs.DOScale(vector09, duration / 3));
            mainSequence.Append(trs.DOScale(vector10, duration / 3));
            mainSequence.OnComplete(() =>
            {
                if (actionCallBack != null)
                {
                    actionCallBack();
                }
            });

            return mainSequence;
        }
    }
}
