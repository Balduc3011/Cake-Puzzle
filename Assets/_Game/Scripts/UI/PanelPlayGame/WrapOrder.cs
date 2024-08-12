using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class WrapOrder : MonoBehaviour
{
    [SerializeField] List<SlotOrder> slotOrders = new();
    [SerializeField] RectTransform rectWrapSlotOrder;
    [SerializeField] Transform trsBox;

    Vector2 vectorScale = Vector2.zero;
    Vector2 vectorDefault = Vector2.zero;
    Vector3 vectorPunch = new Vector3(.5f, .5f, .5f);

    Sequence mySequence;

    private void Awake()
    {
        vectorDefault = rectWrapSlotOrder.sizeDelta;
        RandomOrder();
    }

    public void RandomOrder() {
        if (mySequence != null)
            mySequence.Kill();
        mySequence = DOTween.Sequence();
        mySequence.Append(rectWrapSlotOrder.DOSizeDelta(vectorDefault, .25f, true));
        for (int i = 0; i < slotOrders.Count; i++)
        {
            int cakeID = ProfileManager.Instance.playerData.cakeSaveData.GetRandomOwnedCake();
            CakeData cakeData = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(cakeID);
            slotOrders[i].InitData(cakeData);
        }
    }

    public void OnOrderComplete() {
        ProfileManager.Instance.playerData.playerResourseSave.AddMoney(15);

        vectorScale = rectWrapSlotOrder.sizeDelta;
        vectorScale.x = -Screen.width;
        if (mySequence != null)
            mySequence.Kill();
        mySequence = DOTween.Sequence();
        mySequence.Append(rectWrapSlotOrder.DOSizeDelta(vectorScale, .25f, true));
        mySequence.Append(trsBox.DOPunchScale(vectorPunch, .25f));
        mySequence.OnComplete(()=> {
            CoinEffect coinEffect = GameManager.Instance.objectPooling.GetCoinEffect();
            coinEffect.transform.position = trsBox.position;
            coinEffect.Move(UIManager.instance.panelTotal.GetCoinTrs());
            RandomOrder();
        });
    }

    public void DoneACake(int cakeID)
    {
        for (int i = 0; i < slotOrders.Count; i++)
        {
            if (slotOrders[i].IsThatCake(cakeID) && !slotOrders[i].IsDone())
            { 
                slotOrders[i].OrderDone();
                break;
            }
        }

        for (int i = 0; i < slotOrders.Count; i++)
        {
            if (!slotOrders[i].IsDone())
                return;
        }

        OnOrderComplete();
    }
}
