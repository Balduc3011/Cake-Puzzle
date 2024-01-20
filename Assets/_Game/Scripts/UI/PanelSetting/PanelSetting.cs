using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSetting : UIPanel
{
    [SerializeField] Button closeBtn;
    [SerializeField] Button toMenuBtn;
    public override void Awake()
    {
        panelType = UIPanelType.PanelSetting;
        base.Awake();
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClose);
        toMenuBtn.onClick.AddListener(OnClose);
    }

    void OnClose()
    {
        openAndCloseAnim.OnClose(UIManager.instance.ClosePanelSetting);
    }
}
