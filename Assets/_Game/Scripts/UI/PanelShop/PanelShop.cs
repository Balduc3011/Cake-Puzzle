using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShop : UIPanel
{
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    [SerializeField] SheetAnimation sheetAnimation;
    [SerializeField] RectTransform contentRect;
    public override void Awake()
    {
        panelType = UIPanelType.PanelShop;
        base.Awake();
    }

    private void OnEnable()
    {
        sheetAnimation.PlayAnim();
    }

    public void OnClose()
    {
        uiPanelShowUp.OnClose(UIManager.instance.ClosePanelShop);
    }

}
