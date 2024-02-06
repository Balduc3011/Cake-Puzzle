using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelUsingItem : UIPanel
{
    [SerializeField] Image iconItem;
    [SerializeField] TextMeshProUGUI txtTitle;
    [SerializeField] TextMeshProUGUI txtDescript;
    public override void Awake()
    {
        panelType = UIPanelType.PanelUsingItem;
        base.Awake();
    }
    ItemDataCF currentItemData;
    public void OnUsingItem(ItemType itemType) {
        currentItemData = ProfileManager.Instance.dataConfig.itemDataConfig.GetItemData(itemType);
        txtTitle.text = currentItemData.title;
        txtDescript.text = currentItemData.description;
        iconItem.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(itemType);
        
        UIManager.instance.panelTotal.UsingItemMode();
        UIManager.instance.panelGamePlay.UsingItemMode();
    }

    public void UsingItemDone() {
        UIManager.instance.panelGamePlay.OutItemMode();
        UIManager.instance.panelTotal.OutItemMode();
        UIManager.instance.ClosePanelUsingItem();
    }
}