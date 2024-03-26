using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    [SerializeField] Bomb bombPref;
    [SerializeField] Transform itemTrs;
    [SerializeField] Transform pointBombIn;
    [SerializeField] Transform pointFillUpTarget;

    PanelUsingItem panelUsingItem;
    Transform itemTrsSpawned = null;

    [SerializeField] Transform objHammer;
    [SerializeField] Animator hammerAnim;
    [SerializeField] Vector3 vectorHammerOffset;

    public bool isUsingItem = false;

    public Vector3 GetPointItemIn()
    {
        return pointBombIn.position;
    }

    public void UsingItem(ItemType itemType) {
        if (ProfileManager.Instance.playerData.playerResourseSave.IsHaveItem(itemType)) {
            isUsingItem = true;
            switch (itemType)
            {
                case ItemType.None:
                    break;
                case ItemType.Trophy:
                    break;
                case ItemType.Coin:
                    break;
                case ItemType.Swap:
                    break;
                case ItemType.Bomb:
                    break;
                case ItemType.ReRoll:
                    GameManager.Instance.cakeManager.UsingReroll();
                    ProfileManager.Instance.playerData.playerResourseSave.UsingItem(itemType);
                    break;
                case ItemType.Hammer:
                    UIManager.instance.ShowPanelUsingItem();
                    UsingItemWithPanel(ItemType.Hammer);
                    EventManager.TriggerEvent(EventName.UsingHammer.ToString());
                    break;
                case ItemType.FillUp:
                    UIManager.instance.ShowPanelUsingItem();
                    UsingItemWithPanel(ItemType.FillUp);
                    EventManager.TriggerEvent(EventName.UsingFillUp.ToString());
                    break;
                default:
                    break;
            }
        }
    }

    void UsingItemWithPanel(ItemType itemType) {
        if (panelUsingItem == null) { panelUsingItem = UIManager.instance.GetPanel(UIPanelType.PanelUsingItem).GetComponent<PanelUsingItem>(); }
        GameManager.Instance.cameraManager.UsingItemMode();
        GameManager.Instance.lightManager.UsingItemMode();
        panelUsingItem.OnUsingItem(itemType);
    }

    public void UsingItemDone() {
        isUsingItem = false;
        if (panelUsingItem == null) { panelUsingItem = UIManager.instance.GetPanel(UIPanelType.PanelUsingItem).GetComponent<PanelUsingItem>(); }
        panelUsingItem.UsingItemDone();
        if (itemTrsSpawned != null)
        {
            itemTrsSpawned.DOMove(itemTrs.position, 1f).SetEase(Ease.InCubic);
        }
        GameManager.Instance.cameraManager.OutItemMode();
        GameManager.Instance.lightManager.OutItemMode();
    }

    public void CallUsingHammerOnCake(Cake cake, UnityAction actionCallBack) {
        objHammer.DOMove(cake.transform.position + vectorHammerOffset, .25f).SetEase(Ease.OutQuint).OnComplete(()=> {
            hammerAnim.Play("HammerBam");
            DOVirtual.DelayedCall(0.6f, ()=> {
                Transform trsSmoke = GameManager.Instance.objectPooling.GetSmokeEffect();
                trsSmoke.transform.position = cake.transform.position + vectorHammerOffset;
                trsSmoke.gameObject.SetActive(true);
                GameManager.Instance.cameraManager.ShakeCamera(.2f);
                DOVirtual.DelayedCall(.3f, () =>
                {
                    //trsSmoke.gameObject.SetActive(false);
                    GameManager.Instance.cameraManager.OutItemMode();
                    GameManager.Instance.lightManager.OutItemMode();
                    panelUsingItem.UsingItemDone();
                    objHammer.DOMove(itemTrs.position, .25f);
                    actionCallBack();
                });
                
            });
        });
    }

    public Vector3 GetPointFillUp()
    {
        return pointFillUpTarget.position;
    }
}
