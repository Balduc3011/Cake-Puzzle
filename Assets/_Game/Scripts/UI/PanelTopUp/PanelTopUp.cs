using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIAnimation;
using DG.Tweening;

public class PanelTopUp : UIPanel
{
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    public SheetAnimation sheetAnimation;

    public override void Awake()
    {
        panelType = UIPanelType.PanelTopUp;
        base.Awake();
    }

    private void OnEnable()
    {
        sheetAnimation.PlayAnim();
    }

    public void OnClose()
    {
        uiPanelShowUp.OnClose(UIManager.instance.ClosePanelTopUp);
    }
}
