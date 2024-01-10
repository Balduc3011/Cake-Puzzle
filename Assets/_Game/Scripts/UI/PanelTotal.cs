using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTotal : UIPanel
{
    [SerializeField] Button dailyBtn;
    [SerializeField] Button spinBtn;
    public override void Awake()
    {
        panelType = UIPanelType.PanelTotal;
        base.Awake();
    }
    void Start()
    {
        dailyBtn.onClick.AddListener(ShowPanelDailyReward);
        spinBtn.onClick.AddListener(ShowPanelSpin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowPanelDailyReward()
    {
        UIManager.instance.ShowPanelDailyReward();
    }

    void ShowPanelSpin()
    {
        UIManager.instance.ShowPanelSpin();
    }
}
