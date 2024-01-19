using System.Collections;
using System.Collections.Generic;
using UIAnimation;
using UnityEngine;
using UnityEngine.UI;

public class PanelTotal : UIPanel
{
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    public Transform Transform;
    [SerializeField] Button playBtn;
    [SerializeField] Button settingBtn;
    [SerializeField] Button dailyBtn;
    [SerializeField] Button spinBtn;
    [SerializeField] Button decorBtn;
    [SerializeField] Button mainGameNavBtn;
    [SerializeField] Button bakeryNavBtn;
    [SerializeField] GameObject mainSceneContent;
    [SerializeField] GameObject commonContent;
    [SerializeField] GameObject mainMenuContent;
    [SerializeField] GameObject navBarContent;
    public override void Awake()
    {
        panelType = UIPanelType.PanelTotal;
        base.Awake();
    }
    void Start()
    {
        playBtn.onClick.AddListener(PlayGame);
        settingBtn.onClick.AddListener(ShowPanelSetting);
        dailyBtn.onClick.AddListener(ShowPanelDailyReward);
        spinBtn.onClick.AddListener(ShowPanelSpin);
        decorBtn.onClick.AddListener(ShowPanelDecor);
        mainGameNavBtn.onClick.AddListener(() => { UIManager.instance.ShowPanelTotalContent(); });
        bakeryNavBtn.onClick.AddListener(() => { UIManager.instance.ShowPanelBakery(); });
    }

    public void ShowMainSceneContent(bool show)
    {
        mainSceneContent.gameObject.SetActive(show);
    }

    void PlayGame()
    {
        GameManager.Instance.PlayGame();
        navBarContent.SetActive(false);
        mainMenuContent.SetActive(false);
    }

    void BackToMenu()
    {
        navBarContent.SetActive(true);
        mainMenuContent.SetActive(true);
    }

    void ShowPanelSetting()
    {
        UIAnimationController.BasicButton(settingBtn.transform, .1f, UIManager.instance.ShowPanelSetting);
        //UIManager.instance.ShowPanelSpin();
    }

    void ShowPanelDailyReward()
    {
        UIAnimationController.BasicButton(dailyBtn.transform, .1f, UIManager.instance.ShowPanelDailyReward);
        //UIManager.instance.ShowPanelDailyReward();
    }

    void ShowPanelSpin()
    {
        UIAnimationController.BasicButton(spinBtn.transform, .1f, UIManager.instance.ShowPanelSpin);
        //UIManager.instance.ShowPanelSpin();
    }
    void ShowPanelDecor()
    {
        UIAnimationController.BasicButton(decorBtn.transform, .1f, UIManager.instance.ShowPanelSpin);
        //UIManager.instance.ShowPanelSpin();
    }
}
