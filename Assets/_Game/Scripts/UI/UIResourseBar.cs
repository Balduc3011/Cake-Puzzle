using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIResourseBar : MonoBehaviour
{
    [SerializeField] ItemType itemType;
    [SerializeField] TextMeshProUGUI amountTxt;
    [SerializeField] Image imgCoin;
    Sequence mySequence;
    void Start()
    {
        switch (itemType)
        {
            case ItemType.None:
                break;
            case ItemType.Trophy:
                EventManager.AddListener(EventName.ChangeCoin.ToString(), UpdateValue);
                break;
            case ItemType.Coin:
                EventManager.AddListener(EventName.ChangeCoin.ToString(), UpdateValue);
                break;
            case ItemType.Swap:
                break;
            case ItemType.Hammer:
                break;
            case ItemType.ReRoll:
                break;
            default:
                break;
        }
        UpdateValue();
    }

    void UpdateValue()
    {
        switch (itemType)
        {
            case ItemType.None:
                break;
            case ItemType.Trophy:
                amountTxt.text = ProfileManager.Instance.playerData.playerResourseSave.trophy.ToString();
                break;
            case ItemType.Coin:
                amountTxt.text = ProfileManager.Instance.playerData.playerResourseSave.coins.ToString();
                break;
            case ItemType.Swap:
                break;
            case ItemType.Hammer:
                break;
            case ItemType.ReRoll:
                break;
            default:
                break;
        }
        //AnimChangeCoin();
    }

    public void AnimChangeCoin() {
        if (imgCoin == null)
            return;
        if (mySequence != null)
            mySequence.Kill();
        mySequence = DOTween.Sequence();
        imgCoin.transform.localScale = Vector3.one;
        mySequence.Append(imgCoin.transform.DOScale(Vector3.one * .8f, .12f).SetEase(Ease.InQuad));
        mySequence.Append(imgCoin.transform.DOScale(Vector3.one * 1.2f, .15f).SetEase(Ease.InQuad));
        mySequence.Append(imgCoin.transform.DOScale(Vector3.one, .15f).SetEase(Ease.OutQuad));
        mySequence.Play();
    }
}
