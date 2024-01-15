using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTotal : UIPanel
{
    [SerializeField] Button playBtn;
    [SerializeField] Button dailyBtn;
    [SerializeField] Button spinBtn;
    public override void Awake()
    {
        panelType = UIPanelType.PanelTotal;
        base.Awake();
    }
    void Start()
    {
        playBtn.onClick.AddListener(PlayGame);
        dailyBtn.onClick.AddListener(ShowPanelDailyReward);
        spinBtn.onClick.AddListener(ShowPanelSpin);
    }

    void PlayGame()
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
