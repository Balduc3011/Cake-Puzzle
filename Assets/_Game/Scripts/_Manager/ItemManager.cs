using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] Bomb bombPref;
    [SerializeField] Transform itemTrs;
    [SerializeField] Transform pointBombIn;

    PanelUsingItem panelUsingItem;
    Transform itemTrsSpawned;

    public Vector3 GetPointItemIn()
    {
        return pointBombIn.position;
    }

    public void UsingItem(ItemType itemType) {
        if (ProfileManager.Instance.playerData.playerResourseSave.IsHaveItem(itemType)){
            switch (itemType)
            {
                case ItemType.None:
                    break;
                case ItemType.Gem:
                    break;
                case ItemType.Coin:
                    break;
                case ItemType.Swap:
                    break;
                case ItemType.Hammer:
                    break;
                case ItemType.ReRoll:
                    GameManager.Instance.cakeManager.UsingReroll();
                    break;
                case ItemType.Bomb:
                    UIManager.instance.ShowPanelUsingItem();
                    itemTrsSpawned = Instantiate(bombPref, itemTrs).transform;
                    itemTrsSpawned.DOMove(pointBombIn.position, 1f).SetEase(Ease.InCubic);
                    UsingItemWithPanel(ItemType.Bomb);
                    break;
                case ItemType.FillUp:
                    UIManager.instance.ShowPanelUsingItem();
                    UsingItemWithPanel(ItemType.FillUp);
                    GameManager.Instance.cakeManager.UsingFilUp();
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
        if (panelUsingItem == null) { panelUsingItem = UIManager.instance.GetPanel(UIPanelType.PanelUsingItem).GetComponent<PanelUsingItem>(); }
        panelUsingItem.UsingItemDone();
        GameManager.Instance.cameraManager.OutItemMode();
        GameManager.Instance.lightManager.OutItemMode();
    }
}
