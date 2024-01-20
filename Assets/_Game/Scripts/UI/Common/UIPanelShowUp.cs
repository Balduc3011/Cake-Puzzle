using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPanelShowUp : MonoBehaviour
{
    public bool startOnAwake = true;
    public FloatDirection floatDirection;
    public CanvasGroup canvasGroup;
    public Transform panelTrs;
    Vector3 showPosition;
    Vector3 hidePosition;

    void Awake()
    {
        showPosition = panelTrs.position;
        SetUpPosition();
    }

    void SetUpPosition()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        switch (floatDirection)
        {
            case FloatDirection.None:
                hidePosition = showPosition;
                break;
            case FloatDirection.Up:
                hidePosition = new Vector3(showPosition.x, -screenSize.y / 2, 0);
                break;
            case FloatDirection.Down:
                hidePosition = new Vector3(showPosition.x, 3 * screenSize.y / 2, 0);
                break;
            case FloatDirection.Left:
                hidePosition = new Vector3(-screenSize.x / 2, showPosition.y, 0);
                break;
            case FloatDirection.Right:
                hidePosition = new Vector3(3 * screenSize.x / 2, showPosition.y, 0);
                break;
            default:
                hidePosition = showPosition;
                break;
        }
    }

    private void OnEnable()
    {
        if(startOnAwake)
        {
            panelTrs.position = hidePosition;
            canvasGroup.alpha = 0;
            panelTrs.DOMove(showPosition, 0.25f);
            canvasGroup.DOFade(1, 0.25f);
        }
        startOnAwake = true;
    }

    public void OnClose(UnityAction actionDone = null)
    {
        panelTrs.DOMove(hidePosition, 0.25f);
        canvasGroup.DOFade(0, 0.25f).OnComplete(() =>
        {
            if(actionDone != null)
                actionDone();
        }) ;
    }
}

public enum FloatDirection
{
    None = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
}
