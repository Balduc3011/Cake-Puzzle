using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class UIPanelAnimOpenAndClose : MonoBehaviour
{
    [Header("Animation")]
    public Transform trsButton;
    public CanvasGroup canvasGroup;
    public CanvasGroup BGCanvasGroup;
    public RectTransform trsWrapPanel;
    public Vector3 rotageTarget = new Vector3(0, 0, 3f);
    public Vector3 offset = new Vector3(0, 100f, 0);
    public Vector3 vectorScaleOff = new Vector3(.5f, .5f, 1f);
    public Vector3 vectorScalePunch = new Vector3(.2f, .2f, 0f);
    [HideInInspector] public Vector3 moveTarget;
    [HideInInspector] public Vector3 moveTargetOffset;
    Vector3 vectorScaleDefault = new Vector3(1f, 1f, 1f);

    private void OnEnable()
    {
        if (moveTarget == Vector3.zero && trsWrapPanel != null) moveTarget = trsWrapPanel.position;
        moveTargetOffset = moveTarget + offset;

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (BGCanvasGroup != null)
            BGCanvasGroup.alpha = 0;

        if (trsWrapPanel != null)
        {
            trsWrapPanel.position = new Vector3(transform.position.x, 0, transform.position.z);
            trsWrapPanel.DOMove(moveTargetOffset, .25f).OnComplete(() => {
                trsWrapPanel.DOMove(moveTarget, .25f);
            });
            trsWrapPanel.DORotate(rotageTarget, .25f).OnComplete(() => {
                trsWrapPanel.DORotate(Vector3.zero, .25f);
            });
        }

        canvasGroup.DOFade(1, .5f).OnComplete(() => {
            //if (BGCanvasGroup != null) BGCanvasGroup.DOFade(1, 0.25f);
        });
        if (BGCanvasGroup != null) BGCanvasGroup.DOFade(1, 0.5f);
    }

    public void OnClose(UnityAction actionDone = null) {
        if (canvasGroup != null) canvasGroup.DOFade(0, 0.15f);
        if (BGCanvasGroup != null) BGCanvasGroup.DOFade(0, 0.15f);
        if (trsWrapPanel == null)
        {
            StartCoroutine(IE_DoActionDone(actionDone));
            return;
        }
        trsWrapPanel.DOScale(vectorScaleOff, 0.25f).OnComplete(() => {
            trsWrapPanel.localScale = vectorScaleDefault;
        });
        StartCoroutine(IE_DoActionDone(actionDone));
    }

    IEnumerator IE_DoActionDone(UnityAction actionDone = null) {
        yield return new WaitForSeconds(0.25f);
        if (actionDone != null)
            actionDone();
    }

    public void ButtonActive() {
        trsButton.localScale = vectorScaleDefault;
        trsButton.DOScale(vectorScalePunch, 0.05f).OnComplete(()=> {
            trsButton.DOScale(vectorScaleDefault, 0.05f);
        });
    }
}
