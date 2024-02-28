using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

namespace UIAnimation
{
    public static class UIAnimationController {
        static Vector3 vectorDefault = Vector3.one;
        static Vector3 vectorRotate3 = new Vector3(0, 0, 3);
        static Vector3 vectorRotate0 = Vector3.zero;
        static Vector3 vectorScaleTo09 = new Vector3(.9f, .9f, .9f);
        static Vector3 vectorScaleTo08 = new Vector3(.8f, .8f, .8f);
        static Vector3 vectorScaleTo11 = new Vector3(1.1f, 1.1f, 1.1f);
        static Vector3 vectorScaleTo12 = new Vector3(1.2f, 1.2f, 1.2f);
        static Vector3 vectorScaleTo15 = new Vector3(1.5f, 1.5f, 1.5f);
        static Vector3 vectorMove;
        static Vector3 vectorStart;
        public static void BtnAnimType1(Transform trsDoAnim, float duration, UnityAction actioncallBack = null)
        {
            Sequence mainSquence = DOTween.Sequence();
            Sequence scaleSequence = DOTween.Sequence();
            scaleSequence.Append(trsDoAnim.DOScale(vectorScaleTo09, duration / 2));
            scaleSequence.Join(trsDoAnim.DORotate(-vectorRotate3, duration / 2));
            scaleSequence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            scaleSequence.Join(trsDoAnim.DORotate(vectorRotate0, duration / 2));

            mainSquence.Append(scaleSequence);
            mainSquence.Play();
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }

        public static void BtnAnimZoomBasic(Transform trsDoAnim, float duration, UnityAction actioncallBack = null)
        {
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo09, duration / 2));
            mainSquence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            mainSquence.Play();
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }
        public static Sequence BtnAnimZoomBasic(Transform trsDoAnim, float duration, int loop, UnityAction actioncallBack = null)
        {
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo15, duration));
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo12, duration));
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo15, duration));
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo12, duration));
            mainSquence.SetLoops(loop);
            mainSquence.Play();
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
            return mainSquence;
        }

        public static void PanelPopUpBasic(Transform trsDoAnim, float duration, UnityAction actioncallBack = null)
        {
            trsDoAnim.localScale = vectorDefault;
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo11, duration / 2));
            mainSquence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            mainSquence.Play();
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }

        public static void PanelPopUpBasic(Transform trsDoAnim, float duration, bool loop, UnityAction actioncallBack = null)
        {
            trsDoAnim.localScale = vectorScaleTo08;
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo11, duration / 2));
            mainSquence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            mainSquence.Play();
            mainSquence.SetLoops(loop ? -1 : 0);
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }

        public static Sequence PopupBigZoom(Transform trsDoAnim, float duration, bool loop, UnityAction actioncallBack = null)
        {
            trsDoAnim.localScale = vectorScaleTo08;
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo12, duration / 2));
            mainSquence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            mainSquence.Play();
            mainSquence.SetLoops(loop ? -1 : 0);
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
            return mainSquence;
        }

        public static void PopupBigZoomLoop(Transform trsDoAnim, float duration, UnityAction actioncallBack = null)
        {
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo12, duration / 2));
            mainSquence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            mainSquence.Play();
            mainSquence.SetLoops(-1);
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }

        public static Sequence MoveUpAndDown(Transform trsMove, Vector3 vectorOffset, float duration)
        {
            Sequence mainSequence = DOTween.Sequence();
            vectorMove = trsMove.position + vectorOffset;
            vectorStart = trsMove.position;
            mainSequence.Append(trsMove.DOMove(vectorMove, duration));
            mainSequence.Append(trsMove.DOMove(vectorStart, duration));
            mainSequence.SetLoops(-1);
            mainSequence.Play();
            return mainSequence;
        }
    }
}
