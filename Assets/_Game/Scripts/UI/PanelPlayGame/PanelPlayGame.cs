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
    [SerializeField] Button btnFillUp;
    [SerializeField] Button btnReroll;
    [SerializeField] List<TransitionUI> transitionUIList;
    public override void Awake()
    {
        panelType = UIPanelType.PanelPlayGame;
        base.Awake();
        btnItemBomb.onClick.AddListener(UsingItemBomb);
        btnFillUp.onClick.AddListener(UsingItemFillUp);
        btnReroll.onClick.AddListener(UsingReroll);
    }

    void UsingItemBomb() {
        GameManager.Instance.itemManager.UsingItem(ItemType.Bomb);
    }

    void UsingItemFillUp() {
        GameManager.Instance.itemManager.UsingItem(ItemType.FillUp);
    }

    void UsingReroll() {
        GameManager.Instance.itemManager.UsingItem(ItemType.ReRoll);
    }

    public void UsingItemMode()
    {
        for (int i = 0; i < transitionUIList.Count; i++)
        {
            transitionUIList[i].OnShow(false);
        }
    }

    public void OutItemMode()
    {
        for (int i = 0; i < transitionUIList.Count; i++)
        {
            transitionUIList[i].OnShow(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UsingItemMode();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            OutItemMode();
        }
    }
}
