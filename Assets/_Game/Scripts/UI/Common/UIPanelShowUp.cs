using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPanelShowUp : MonoBehaviour
{
    [SerializeField] bool overflow;
    public bool startOnAwake = true;
    public FloatDirection floatDirection;
    public CanvasGroup canvasGroup;
    public Transform panelTrs;
    Vector3 showPosition;
    Vector3 hidePosition;

    public List<PopUpTrs> popUpTrs;

    void Awake()
    {
        if (panelTrs != null)
        {
            SetUpPosition();
        }
        else
        {
            SetupPopupTrs();
        }
    }

    void SetUpPosition()
    {
        showPosition = panelTrs.position;
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
            if(floatDirection == FloatDirection.None)
            {
                if (panelTrs != null)
                {
                    PopOut();
                }
                else
                {
                    PopOutAll();
                }
                canvasGroup.DOFade(1, 0.25f).From(0);
            }
            else
            {
                if(panelTrs != null)
                {
                    panelTrs.position = hidePosition;
                    panelTrs.DOMove(showPosition, 0.25f).SetEase(overflow ? Ease.OutBack : Ease.InOutQuad);
                }
                else
                {
                    MoveTrs();
                }
                canvasGroup.DOFade(1, 0.25f).From(0);
            }
        }
        startOnAwake = true;
    }

    void PopOut()
    {
        panelTrs.DOScale(1, 0.25f).SetEase(overflow ? Ease.OutBack : Ease.InOutQuad).From(0);
        //canvasGroup.DOFade(1, 0.25f).From(0);
    }

    public void OnClose(UnityAction actionDone = null)
    {
        if (panelTrs != null)
        {
            panelTrs.DOMove(hidePosition, 0.25f);
        }
        else
        {
            if (floatDirection == FloatDirection.None)
            {
                PopInAll();
            }
            else
            {
                HideTrs();
            }
        }
        canvasGroup.DOFade(0, 0.25f).OnComplete(() =>
        {
            if(actionDone != null)
                actionDone();
        }) ;
    }

    void SetupPopupTrs()
    {
        for (int i = 0; i < popUpTrs.Count; i++)
        {
            SetUpPosition(popUpTrs[i]);
        }
    }

    void SetUpPosition(PopUpTrs popUp)
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        popUp.showPosition = popUp.panelTrs.position;
        switch (floatDirection)
        {
            case FloatDirection.None:
                popUp.hidePosition = showPosition;
                break;
            case FloatDirection.Up:
                popUp.hidePosition = new Vector3(showPosition.x, -screenSize.y / 2, 0);
                break;
            case FloatDirection.Down:
                popUp.hidePosition = new Vector3(showPosition.x, 3 * screenSize.y / 2, 0);
                break;
            case FloatDirection.Left:
                popUp.hidePosition = new Vector3(-screenSize.x / 2, showPosition.y, 0);
                break;
            case FloatDirection.Right:
                popUp.hidePosition = new Vector3(3 * screenSize.x / 2, showPosition.y, 0);
                break;
            default:
                popUp.hidePosition = showPosition;
                break;
        }
    }

    void MoveTrs()
    {
        for (int i = 0; i < popUpTrs.Count; i++)
        {
            popUpTrs[i].panelTrs.position = popUpTrs[i].hidePosition;
            popUpTrs[i].panelTrs.DOMove(popUpTrs[i].showPosition, 0.25f).SetEase(overflow ? Ease.OutBack : Ease.InOutQuad).SetDelay(0.1f * i);
        }
    }

    void HideTrs()
    {
        for (int i = 0; i < popUpTrs.Count; i++)
        {
            popUpTrs[i].panelTrs.DOMove(popUpTrs[i].hidePosition, 0.25f).SetEase(overflow ? Ease.OutBack : Ease.InOutQuad).SetDelay(0.1f * i);
        }
    }

    void PopOutAll()
    {
        for (int i = 0; i < popUpTrs.Count; i++)
        {
            popUpTrs[i].panelTrs.DOScale(1, 0.25f).SetEase(overflow ? Ease.OutBack : Ease.InOutQuad).From(0).SetDelay(0.1f * i);
        }
    }
    void PopInAll()
    {
        for (int i = 0; i < popUpTrs.Count; i++)
        {
            popUpTrs[i].panelTrs.DOScale(0, 0.15f).SetEase(overflow ? Ease.OutBack : Ease.InOutQuad).SetDelay(0.1f * i);
        }
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
[System.Serializable]
public class PopUpTrs
{
    public Transform panelTrs;
    public Vector3 showPosition;
    public Vector3 hidePosition;
}
