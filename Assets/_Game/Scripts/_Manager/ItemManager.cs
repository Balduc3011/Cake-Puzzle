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
                case ItemType.Revive:
                    UIManager.instance.ShowPanelUsingItem();
                    UsingItemWithPanel(ItemType.Revive);
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
        panelUsingItem.OnUseItem(itemType);
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
                    UsingItemDone();
                    panelUsingItem.UsingItemDone();
                    objHammer.DOMove(itemTrs.position, .25f);
                    actionCallBack();
                });
                
            });
        });
    }

    public void OnUsingItem() {
        panelUsingItem.OnUsingItem();
    }

    public Vector3 GetPointFillUp()
    {
        return pointFillUpTarget.position;
    }
    UnityAction actionCallBack;
    public void Revie(Cake cake, UnityAction actionCallBack, bool lastItem = false) {
        this.actionCallBack = actionCallBack;
        objHammer.DOMove(cake.transform.position + vectorHammerOffset, .25f).SetEase(Ease.OutQuint).OnComplete(() => {
            hammerAnim.Play("HammerBam");
            DOVirtual.DelayedCall(0.6f, () => {
                Transform trsSmoke = GameManager.Instance.objectPooling.GetSmokeEffect();
                trsSmoke.transform.position = cake.transform.position + vectorHammerOffset;
                trsSmoke.gameObject.SetActive(true);
                GameManager.Instance.cameraManager.ShakeCamera(.2f);
                DOVirtual.DelayedCall(.15f, () =>
                {
                    trsSmoke.gameObject.SetActive(false);
                });
                
                if (lastItem)
                {
                    DoLastMove(actionCallBack);
                }
                else
                {
                    actionCallBack();
                }

            });
        });
    }
    public void DoLastMove(UnityAction actionCallBack)
    {
        UsingItemDone();
        panelUsingItem.UsingItemDone();
        EventManager.TriggerEvent(EventName.OnUsingReviveDone.ToString());
        objHammer.DOMove(itemTrs.position, .25f).OnComplete(() => {
            actionCallBack();
        });
    }
    PlateIndex plateIndex;
    PlateIndex plateUpIndex;
    PlateIndex plateDownIndex;
    PlateIndex plateLeftIndex;
    PlateIndex plateRightIndex;

    int currentIndex = 0;
    public void AssignCakeCallBack(Cake cakeCallBack) { 
        plateIndex = cakeCallBack.currentPlate.plateIndex;
        plateUpIndex = new(cakeCallBack.currentPlate.plateIndex);
        plateDownIndex = new(cakeCallBack.currentPlate.plateIndex);
        plateRightIndex = new(cakeCallBack.currentPlate.plateIndex);
        plateLeftIndex = new(cakeCallBack.currentPlate.plateIndex);
        currentIndex = 0;
    }
    public void RemoveCake()
    {
        currentIndex++;
        switch (currentIndex)
        {
            case 1:
                if (plateUpIndex.indexX - 1 >= 0)
                {
                    //DOVirtual.DelayedCall(.8f, () =>
                    //{
                        plateUpIndex.indexX--;
                        //Debug.Log("Plate Up: " + plateUpIndex.indexX + " " + plateUpIndex.indexY);
                        ProfileManager.Instance.playerData.cakeSaveData.RemoveCake(plateUpIndex);
                    if (GameManager.Instance.cakeManager.table.plateArray[plateUpIndex.indexX, plateUpIndex.indexY].currentCake != null)
                        GameManager.Instance.cakeManager.table.plateArray[plateUpIndex.indexX, plateUpIndex.indexY].currentCake.UsingRevive();
                    else
                    {
                        RemoveCake(); 
                    }
                    //});
                }
                else
                {
                    RemoveCake();
                }
                break;
            case 2:
                if (plateDownIndex.indexX + 1 < 5)
                {
                    //DOVirtual.DelayedCall(.8f, () =>
                    //{
                        plateDownIndex.indexX++;
                        //Debug.Log("Plate Down: " + plateDownIndex.indexX + " " + plateDownIndex.indexY);
                        ProfileManager.Instance.playerData.cakeSaveData.RemoveCake(plateDownIndex);
                    if (GameManager.Instance.cakeManager.table.plateArray[plateDownIndex.indexX, plateDownIndex.indexY].currentCake != null)
                        GameManager.Instance.cakeManager.table.plateArray[plateDownIndex.indexX, plateDownIndex.indexY].currentCake?.UsingRevive();
                    else
                    {
                        RemoveCake();
                    }
                    //});
                }
                else
                {
                    RemoveCake();
                }
                break;
            case 3:
                if (plateLeftIndex.indexY - 1 >= 0)
                {
                    //DOVirtual.DelayedCall(.8f, () =>
                    //{
                    plateLeftIndex.indexY--;
                    //Debug.Log("Plate Left: " + plateLeftIndex.indexX + " " + plateLeftIndex.indexY);
                    ProfileManager.Instance.playerData.cakeSaveData.RemoveCake(plateLeftIndex);
                    if (GameManager.Instance.cakeManager.table.plateArray[plateLeftIndex.indexX, plateLeftIndex.indexY].currentCake != null)
                        GameManager.Instance.cakeManager.table.plateArray[plateLeftIndex.indexX, plateLeftIndex.indexY].currentCake?.UsingRevive();
                    else {
                        RemoveCake();
                    }
                    //});
                }
                else
                {
                    RemoveCake();
                }
                break;
            case 4:
                if (plateRightIndex.indexY + 1 < 4)
                {
                    //DOVirtual.DelayedCall(.8f, () =>
                    //{
                        plateRightIndex.indexY++;
                        //Debug.Log("Plate Right: " + plateRightIndex.indexX + " " + plateRightIndex.indexY);
                        ProfileManager.Instance.playerData.cakeSaveData.RemoveCake(plateRightIndex);
                    if (GameManager.Instance.cakeManager.table.plateArray[plateRightIndex.indexX, plateRightIndex.indexY].currentCake != null)
                        GameManager.Instance.cakeManager.table.plateArray[plateRightIndex.indexX, plateRightIndex.indexY].currentCake?.UsingRevive(true);
                    else {
                        DoLastMove(actionCallBack); 
                    }
                    //});
                }
                else
                {
                    DoLastMove(actionCallBack);
                }
                break;
            default:
                break;
        }
    }
}
