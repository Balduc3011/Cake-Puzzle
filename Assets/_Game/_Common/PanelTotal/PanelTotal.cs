using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] GameObject backGround;
    [SerializeField] TextMeshProUGUI txtCurrentLevel;
    [SerializeField] TextMeshProUGUI txtCurrentExp;
    [SerializeField] Image imgNextCake;
    [SerializeField] Slider sliderLevelExp;
    public override void Awake()
    {
        panelType = UIPanelType.PanelTotal;
        base.Awake();
        EventManager.AddListener(EventName.ChangeLevel.ToString(), ChangeLevel);
        EventManager.AddListener(EventName.ChangeExp.ToString(), ChangeExp);
    }

    float currentExp= 0;
    private void ChangeExp()
    {
        currentExp = ProfileManager.Instance.playerData.playerResourseSave.currentExp;
        txtCurrentExp.text = ProfileManager.Instance.playerData.playerResourseSave.GetCurrentExp();
        sliderLevelExp.value = currentExp;
        sliderLevelExp.maxValue = ProfileManager.Instance.playerData.playerResourseSave.GetMaxExp();
    }
    LevelData levelData;
    private void ChangeLevel()
    {
        txtCurrentLevel.text = ProfileManager.Instance.playerData.playerResourseSave.GetCurrentLevel();
        levelData = ProfileManager.Instance.dataConfig.levelDataConfig.GetLevel(ProfileManager.Instance.playerData.playerResourseSave.currentLevel);
        if (levelData.cakeUnlockID != -1)
        {
            imgNextCake.gameObject.SetActive(true);
            imgNextCake.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetCakeSprite(levelData.cakeUnlockID);
        }
        else { imgNextCake.gameObject.SetActive(false); }
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
        ChangeLevel();
        ChangeExp();
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
        backGround.SetActive(false);
    }

    void BackToMenu()
    {
        navBarContent.SetActive(true);
        mainMenuContent.SetActive(true);
        backGround.SetActive(true);
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
