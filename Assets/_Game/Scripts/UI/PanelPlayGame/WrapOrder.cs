using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using _BaseGame.ScriptableObjects.MapData;
using UnityEngine.Events;

public class WrapOrder : MonoBehaviour
{
    [SerializeField] List<SlotOrder> slotOrders = new();
    [SerializeField] RectTransform rectWrapSlotOrder;
    [SerializeField] Transform trsBox;
    [SerializeField] TextMeshProUGUI txtTime;
    [SerializeField] float timeSetting;

    Vector2 vectorScale = Vector2.zero;
    Vector2 vectorDefault = Vector2.zero;
    Vector3 vectorPunch = new Vector3(.5f, .5f, .5f);

    Sequence mySequence;
    Sequence sequenceTime;

    CakeOrder cakeOrderTemp;

    private void Awake()
    {
        vectorDefault = rectWrapSlotOrder.sizeDelta;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnOrderComplete(false);
        }
    }

    public void GetOrder() {
        Debug.Log("Order");
        if (GameManager.Instance.ordermanager.ShowOrder())
            gameObject.SetActive(true);
        GameManager.Instance.ordermanager.isFail = false;
        if (sequenceTime != null) sequenceTime.Kill();

        sequenceTime = DOTween.Sequence();

        sequenceTime.Append(DOVirtual.Float(timeSetting, 0, timeSetting, (value) => {
            txtTime.text = TimeUtil.TimeToString(((int)value + 1));
        }).SetEase(Ease.Linear).OnComplete(()=> {
            GameManager.Instance.ordermanager.isFail = true;
            UIManager.instance.ShowPanelLevelComplete(false);
            gameObject.SetActive(false);
            //if (!GameManager.Instance.ordermanager.IsDoneAll()) OnOrderComplete(false);
        }));

        if (mySequence != null)
            mySequence.Kill();

        mySequence = DOTween.Sequence();

        mySequence.Append(rectWrapSlotOrder.DOSizeDelta(vectorDefault, .25f, true));

        for (int i = 0; i < slotOrders.Count; i++)
        {
            cakeOrderTemp = GameManager.Instance.ordermanager.GetCakeOrder(i);
            Debug.Log(cakeOrderTemp.amount);
            CakeData cakeData = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(cakeOrderTemp.type);
            slotOrders[i].InitData(cakeData, cakeOrderTemp.amount);
        }
    }

    public void OnOrderComplete(bool isDone, UnityAction actionCallBack = null) {
        

        vectorScale = rectWrapSlotOrder.sizeDelta;
        vectorScale.x = 0;
        if (mySequence != null)
            mySequence.Kill();
        mySequence = DOTween.Sequence();
        mySequence.Append(rectWrapSlotOrder.DOSizeDelta(vectorScale, .25f, true));
        mySequence.Append(trsBox.DOPunchScale(vectorPunch, .25f));
      
        mySequence.OnComplete(() => {
            if (isDone)
            {
                CoinEffect coinEffect = GameManager.Instance.objectPooling.GetCoinEffect();
                coinEffect.transform.position = trsBox.position;
                coinEffect.Move(UIManager.instance.panelTotal.GetCoinTrs());
                if (actionCallBack != null)
                    actionCallBack();
            }
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

       
    }

   
}
