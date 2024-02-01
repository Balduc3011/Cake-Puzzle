using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelPlayGame : UIPanel
{
    [SerializeField] Button x2BoosterBtn;
    [SerializeField] Button coinBoosterBtn;
    [SerializeField] Button btnItemBomb;
    //[SerializeField] Button item2Btn;
    //[SerializeField] Button item3Btn;
    public override void Awake()
    {
        panelType = UIPanelType.PanelPlayGame;
        base.Awake();
        btnItemBomb.onClick.AddListener(UsingItemBomb);
    }

    public void UsingItemBomb() {
        GameManager.Instance.itemManager.UsingItem(ItemType.Bomb);
    }

    public void OutItemMode()
    {
        
    }

    public void UsingItemMode()
    {
        
    }
}
