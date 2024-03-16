using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIAnimation;
using UnityEngine;
using UnityEngine.UI;

public class PanelTotal : UIPanel
{
    public RectTransform subTopRect;
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    public Transform Transform;
    [SerializeField] Button playBtn;
    [SerializeField] Button settingBtn;
    [SerializeField] Button dailyBtn;
    [SerializeField] Button spinBtn;
    [SerializeField] Button decorBtn;
    [SerializeField] Button mainGameNavBtn;
    [SerializeField] Button bakeryNavBtn;
    [SerializeField] Button decorationNavBtn;
    [SerializeField] Button shopNavBtn;
    [SerializeField] Button questNavBtn;
    [SerializeField] GameObject mainSceneContent;
    [SerializeField] GameObject commonContent;
    [SerializeField] GameObject mainMenuContent;
    [SerializeField] GameObject navBarContent;
    //[SerializeField] GameObject backGround;
    [SerializeField] GameObject objBlockAll;
    [SerializeField] TextMeshProUGUI txtCurrentLevel;
    [SerializeField] TextMeshProUGUI txtCurrentExp;
    //[SerializeField] Image imgNextCake;
    [SerializeField] Slider sliderLevelExp;
    [SerializeField] Transform trsCoin;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] List<TransitionUI> transitionUIList;

    [SerializeField] GameObject dailyNoti;
    [SerializeField] GameObject spinNoti;
    [SerializeField] GameObject settingNoti;
    [SerializeField] GameObject bakeryNoti;
    public override void Awake()
    {
        panelType = UIPanelType.PanelTotal;
        base.Awake();
        EventManager.AddListener(EventName.ChangeExp.ToString(), ChangeExp);
        EventManager.AddListener(EventName.AddCakeCard.ToString(), CheckNoti);
        //backGround = UIManager.instance.backGround;
        CheckSubScreenObstacleBase();
    }

    public void CheckNoti()
    {
        dailyNoti.SetActive(ProfileManager.Instance.playerData.playerResourseSave.IsHasDailyReward());
        spinNoti.SetActive(ProfileManager.Instance.playerData.playerResourseSave.IsHasFreeSpin());
        settingNoti.SetActive(ProfileManager.Instance.playerData.cakeSaveData.HasCakeUpgradeable() &&
            GameManager.Instance.playing);
        bakeryNoti.SetActive(ProfileManager.Instance.playerData.cakeSaveData.HasCakeUpgradeable());
    }

    void CheckSubScreenObstacleBase()
    {
        if (subTopRect == null)
        {
            return;
        }
        float screenRatio = (float)Screen.height / (float)Screen.width;
        if (screenRatio > 2.1f) // Now we got problem 
        {
            subTopRect.sizeDelta = new Vector2(0, -100);
            subTopRect.anchoredPosition = new Vector2(0, -50);
        }
        else
        {
            subTopRect.sizeDelta = new Vector2(0, 0);
            subTopRect.anchoredPosition = new Vector2(0, 0);
        }
    }

    float currentExp= 0;
    float currentValue = 0;
    int currentLevel;
    bool isChangeLevel;
    private void ChangeExp()
    {
        if (currentLevel != ProfileManager.Instance.playerData.playerResourseSave.currentLevel)
        {
            currentExp = sliderLevelExp.maxValue;
            isChangeLevel = true;
        }
        else
        {
            isChangeLevel = false;
            currentExp = ProfileManager.Instance.playerData.playerResourseSave.currentExp;
            sliderLevelExp.maxValue = ProfileManager.Instance.playerData.playerResourseSave.GetMaxExp();
        }

        currentValue = sliderLevelExp.value;
        DOVirtual.Float(currentValue, currentExp, 1f, (value) =>{
            sliderLevelExp.value = value;
            txtCurrentExp.text = (int)value + "/" + sliderLevelExp.maxValue;
        }).OnComplete(() => {
            if (isChangeLevel)
            {
                sliderLevelExp.value = 0;
                sliderLevelExp.maxValue = ProfileManager.Instance.playerData.playerResourseSave.GetMaxExp();
                txtCurrentExp.text = 0 + "/" + sliderLevelExp.maxValue;
                currentLevel = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;
                ChangeLevel();
            }
        });
    }
    LevelData levelData;
    private void ChangeLevel()
    {
        
        txtCurrentLevel.text = ProfileManager.Instance.playerData.playerResourseSave.currentLevel.ToString();
        //levelData = ProfileManager.Instance.dataConfig.levelDataConfig.GetLevel(ProfileManager.Instance.playerData.playerResourseSave.currentLevel);
        //if (levelData.cakeUnlockID != -1)
        //{
        //    imgNextCake.gameObject.SetActive(true);
        //    imgNextCake.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetCakeSprite(levelData.cakeUnlockID);
        //}
        //else { 
        //    imgNextCake.gameObject.SetActive(false);
        //}
    }

    void Start()
    {
        playBtn.onClick.AddListener(PlayGame);
        settingBtn.onClick.AddListener(ShowPanelSetting);
        dailyBtn.onClick.AddListener(ShowPanelDailyReward);
        spinBtn.onClick.AddListener(ShowPanelSpin);
        decorBtn.onClick.AddListener(ShowPanelTest);
        mainGameNavBtn.onClick.AddListener(() => { UIManager.instance.ShowPanelTotalContent(); });
        bakeryNavBtn.onClick.AddListener(() => { UIManager.instance.ShowPanelBakery(); });
        decorationNavBtn.onClick.AddListener(() => { UIManager.instance.ShowPanelDecorations(); });
        shopNavBtn.onClick.AddListener(() => { UIManager.instance.ShowPanelShop(); });
        questNavBtn.onClick.AddListener(() => { UIManager.instance.ShowPanelDailyQuest(); });
        currentLevel = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;
        ChangeLevel();
        ChangeExp();
        CheckNoti();
    }

    public void ShowMainSceneContent(bool show)
    {
        mainSceneContent.gameObject.SetActive(show);
    }

    void PlayGame()
    {
        GameManager.Instance.cameraManager.FirstCamera();
        GameManager.Instance.cameraManager.OpenMainCamera();
        GameManager.Instance.PlayGame();
        navBarContent.SetActive(false);
        mainMenuContent.SetActive(false);
        //backGround.SetActive(false);
        CheckNoti();
    }

    public void BackToMenu()
    {
        navBarContent.SetActive(true);
        mainMenuContent.SetActive(true);
        //backGround.SetActive(true);
    }

    void ShowPanelSetting()
    {
        UIAnimationController.BtnAnimZoomBasic(settingBtn.transform, .1f, UIManager.instance.ShowPanelSetting);
        //UIManager.instance.ShowPanelSpin();
    }

    void ShowPanelDailyReward()
    {
        UIAnimationController.BtnAnimZoomBasic(dailyBtn.transform, .1f, UIManager.instance.ShowPanelDailyReward);
        //UIManager.instance.ShowPanelDailyReward();
    }

    void ShowPanelSpin()
    {
        UIAnimationController.BtnAnimZoomBasic(spinBtn.transform, .1f, UIManager.instance.ShowPanelSpin);
        //UIManager.instance.ShowPanelSpin();
    }
    void ShowPanelTest()
    {
        //UIAnimationController.BasicButton(decorBtn.transform, .1f, UIManager.instance.ShowPanelSpin);
        UIAnimationController.BtnAnimZoomBasic(decorBtn.transform, .1f, UIManager.instance.ShowPanelLeaderBoard);
        //UIManager.instance.ShowPanelSpin();
    }

    public Transform GetPointSlider() {
        return sliderLevelExp.handleRect.transform;
    }

    public Transform GetCoinTrs() {
        return trsCoin;
    }

    public void OpenObjBlockAll() { objBlockAll.SetActive(true); }
    public void CloseObjBlockAll() { objBlockAll.SetActive(false); }

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
