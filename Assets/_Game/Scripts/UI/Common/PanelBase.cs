using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : UIPanel
{
    public override void Awake()
    {
        panelType = UIPanelType.PanelBase;
        base.Awake();
    }

    void OnClose()
    {
        UIManager.instance.ClosePanelBase();
    }
}
