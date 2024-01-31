using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShop : UIPanel
{
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    public override void Awake()
    {
        panelType = UIPanelType.PanelShop;
        base.Awake();
    }

    public void OnClose()
    {
        uiPanelShowUp.OnClose(UIManager.instance.ClosePanelShop);
    }

}
