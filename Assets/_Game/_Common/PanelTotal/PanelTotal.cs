using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIAnimation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelTotal : UIPanel
{
    public RectTransform subTopRect;
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    public Transform Transform;
    [SerializeField] CanvasGroup functionCG;
    [SerializeField] CanvasGroup functionCG2;
    [SerializeField] Button playBtn;
    [SerializeField] Button settingBtn;
    [SerializeField] Button dailyBtn;
    [SerializeField] Button dailyQuestBtn;
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
    //[SerializeField] Image imgNextCake;
    [SerializeField] Image sliderLevelExpImg;
    [SerializeField] Slider sliderQuickTimeEvent;
    [SerializeField] Transform trsCoin;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] CanvasGroup quickEventCanvasGroup;
    [SerializeField] List<TransitionUI> transitionUIList;

    [SerializeField] CanvasGroup backGroundCG;
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject functinBar;
    [SerializeField] GameObject dailyNoti;
    [SerializeField] GameObject spinNoti;
    [SerializeField] GameObject settingNoti;
    [SerializeField] GameObject bakeryNoti;
    [SerializeField] GameObject questNoti;
    [SerializeField] GameObject objQuickTimeEvents;

    [SerializeField] TextMeshProUGUI txtCountCake;
    [SerializeField] TextMeshProUGUI txtTime;

    int showingCake = -1;
    public override void Awake()
    {
        panelType = UIPanelType.PanelTotal;
        base.Awake();
        EventManager.AddListener(EventName.ChangeExp.ToString(), ChangeExp);
        EventManager.AddListener(EventName.AddCakeCard.ToString(), CheckNoti);
        //backGround = UIManager.instance.backGround;
        CheckSubScreenObstacleBase();
        Invoke("InitCakeDecor", 0.25f);
    }

    public void CheckNoti()
    {
        dailyNoti.SetActive(ProfileManager.Instance.playerData.playerResourseSave.IsHasDailyReward());
        spinNoti.SetActive(ProfileManager.Instance.playerData.playerResourseSave.IsHasFreeSpin());
        questNoti.SetActive(ProfileManager.Instance.playerData.questDataSave.CheckShowNoticeQuest());
        //settingNoti.SetActive(ProfileManager.Instance.playerData.cakeSaveData.HasCakeUpgradeable() &&
        //    GameManager.Instance.playing);
        settingNoti.SetActive(false);
        bakeryNoti.SetActive(ProfileManager.Instance.playerData.cakeSaveData.HasCakeUpgradeable());
        if(UIManager.instance.panelGamePlay != null)
        {
            UIManager.instance.panelGamePlay.CheckNoti();
        }
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

    float currentExp = 0;
    float currentValue = 0;
    float nextValue = 0;
    int currentLevel;
    bool isChangeLevel;
    float maxExp;
    private void ChangeExp()
    {
        if (currentLevel != ProfileManager.Instance.playerData.playerResourseSave.currentLevel)
        {
            isChangeLevel = true;
        }
        else
        {
            isChangeLevel = false;
        }
        currentValue = sliderLevelExpImg.fillAmount;
        maxExp = ProfileManager.Instance.playerData.playerResourseSave.GetMaxExp();
        currentExp = ProfileManager.Instance.playerData.playerResourseSave.currentExp;
        nextValue = currentExp / maxExp;
        DOVirtual.Float(currentValue, nextValue, 0.35f, (value) => {
            sliderLevelExpImg.fillAmount = value;
        }).OnComplete(() => {
            if (isChangeLevel)
            {
                sliderLevelExpImg.fillAmount = 0;
                currentLevel = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;
                ChangeLevel();
            }
        });
    }
    LevelData levelData;
    private void ChangeLevel()
    {
        txtCurrentLevel.text = ProfileManager.Instance.playerData.playerResourseSave.currentLevel.ToString();
    }

    void Start()
    {
        playBtn.onClick.AddListener(PlayGame);
        settingBtn.onClick.AddListener(ShowPanelSetting);
        dailyBtn.onClick.AddListener(ShowPanelDailyReward);
        dailyQuestBtn.onClick.AddListener(ShowPanelDailyQuest);
        spinBtn.onClick.AddListener(ShowPanelSpin);
        decorBtn.onClick.AddListener(ShowPanelTest);
        mainGameNavBtn.onClick.AddListener(() => {
            GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
            UIManager.instance.ShowPanelTotalContent();
            ShowBGCanvasGroup(true);
            functinBar.SetActive(true);
            mainGameNavBtn.GetComponent<NavBarItem>().ButtonOnClick(2);
            InitCakeDecor();
        });
        bakeryNavBtn.onClick.AddListener(() => {
            GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
            UIManager.instance.ShowPanelBakery();
            ShowBGCanvasGroup(false);
            functinBar.SetActive(false);
            bakeryNavBtn.GetComponent<NavBarItem>().ButtonOnClick(3);
        }); 
        decorationNavBtn.onClick.AddListener(() => {
            GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
            UIManager.instance.ShowPanelDecorations();
            ShowBGCanvasGroup(false);
            functinBar.SetActive(false);
            decorationNavBtn.GetComponent<NavBarItem>().ButtonOnClick(1);
        });
        shopNavBtn.onClick.AddListener(() => {
            GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
            UIManager.instance.ShowPanelShop();
            ShowBGCanvasGroup(false);
            functinBar.SetActive(false);
            shopNavBtn.GetComponent<NavBarItem>().ButtonOnClick(4);
        });
        questNavBtn.onClick.AddListener(() => {
            GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
            UIManager.instance.ShowPanelTopUp();
            ShowBGCanvasGroup(false);
            functinBar.SetActive(false);
            questNavBtn.GetComponent<NavBarItem>().ButtonOnClick(0);
        });
        confirmBuyBtn.onClick.AddListener(OnConfirmShowAds);
        confirmCloseBtn.onClick.AddListener(CloseConfirmShowAds);

        currentLevel = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;
        ChangeLevel();
        ChangeExp();
        CheckNoti();
    }

    void ShowBGCanvasGroup(bool show)
    {
        backGroundCG.DOFade(show ? 1 : 0, show ? 0.1f : .5f);
        functionCG2.DOFade(show ? 1 : 0, 0.15f);
    }

    public void ShowMainSceneContent(bool show)
    {
        mainSceneContent.gameObject.SetActive(show);
    }

    void PlayGame()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        GameManager.Instance.cameraManager.FirstCamera();
        GameManager.Instance.cameraManager.OpenMainCamera();
        //GameManager.Instance.PlayGame();
        navBarContent.SetActive(false);
        mainMenuContent.SetActive(false);
        backGround.SetActive(false);
        functinBar.SetActive(false);
        CheckNoti();
        UIManager.instance.ShowPanelLoading();
        DOVirtual.DelayedCall(2.5f, GameManager.Instance.PlayGame);
    }

    public void BackToMenu()
    {
        navBarContent.SetActive(true);
        mainMenuContent.SetActive(true);
        backGround.SetActive(true);
        functinBar.SetActive(true);
        InitCakeDecor();
    }

    void ShowPanelSetting()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        UIAnimationController.BtnAnimZoomBasic(settingBtn.transform, .1f, UIManager.instance.ShowPanelSetting);
    }

    void ShowPanelDailyReward()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        UIAnimationController.BtnAnimZoomBasic(dailyBtn.transform, .1f, UIManager.instance.ShowPanelDailyReward);
    }
    
    void ShowPanelDailyQuest()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        UIAnimationController.BtnAnimZoomBasic(dailyQuestBtn.transform, .1f, UIManager.instance.ShowPanelDailyQuest);
    }

    void ShowPanelSpin()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        UIAnimationController.BtnAnimZoomBasic(spinBtn.transform, .1f, UIManager.instance.ShowPanelSpin);
    }
    void ShowPanelTest()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        UIAnimationController.BtnAnimZoomBasic(decorBtn.transform, .1f, UIManager.instance.ShowPanelCakeReward);
    }

    public Transform GetPointSlider() {
        return txtCurrentLevel.transform;
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
        showCakeCounter += Time.deltaTime;
        if (showCakeCounter > showCakeCoolDown)
        {
            showCakeCounter = 0;
            InitCakeDecor();
        }
    }

    float showCakeCoolDown = 3 * 60;
    [SerializeField] float showCakeCounter = 0;
    void InitCakeDecor()
    {
        if (GameManager.Instance.playing) return;
        int newShow = ProfileManager.Instance.playerData.cakeSaveData.GetRandomOwnedCake();
        while (newShow == showingCake)
        {
            newShow = ProfileManager.Instance.playerData.cakeSaveData.GetRandomOwnedCake();
        }
        showingCake = newShow;
        GameManager.Instance.cakeManager.cakeShowComponent.ShowSelectetCake(showingCake);
    }

    #region Ads
    [SerializeField] GameObject confirmObj;
    [SerializeField] CanvasGroup confirmCG;
    [SerializeField] Button confirmBuyBtn;
    [SerializeField] Button confirmCloseBtn;
    [SerializeField] TextMeshProUGUI desText;
    [SerializeField] List<GameObject> boostObjs;
    //[SerializeField] Image iconImg;
    UnityAction adsConfirmCallBack;
    public void ShowConfirm(UnityAction unityAction, int index)
    {
        adsConfirmCallBack = unityAction;
        confirmObj.SetActive(true);
        confirmCG.DOFade(1, 0.15f);
        for (int i = 0; i < boostObjs.Count; i++)
        {
            if(i == index)
            {
                boostObjs[i].SetActive(true);
            }
            boostObjs[i].SetActive(false);
        }
    }

    public void CloseConfirmShowAds()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        confirmCG.DOFade(0, 0.15f).OnComplete(CloseConfirmInstant);
    }

    void CloseConfirmInstant()
    {
        confirmObj.SetActive(false);
    }

    void OnConfirmShowAds()
    {
        if (adsConfirmCallBack != null)
        {
            adsConfirmCallBack();
        }
        CloseConfirmShowAds();
    }
    #endregion

    #region Quick Time Event
    public float currentCakeDone;
    float currentTime;

    public void ShowQuickTimeEvent() {
        GameManager.Instance.quickTimeEventManager.InitMission();
        objQuickTimeEvents.SetActive(true);
        quickEventCanvasGroup.DOFade(1, .25f).From(0).SetEase(Ease.InOutSine);
        objQuickTimeEvents.transform.DOScale(1, .25f).From(0).SetEase(Ease.OutBack);
        sliderQuickTimeEvent.maxValue = GameManager.Instance.quickTimeEventManager.GetTotalCakeNeedDone();
        sliderQuickTimeEvent.value = 0;
        currentCakeDone = 0;
        currentTime = GameManager.Instance.quickTimeEventManager.GetTimeQuickTimeEvent();
        txtCountCake.text = "0/" + sliderQuickTimeEvent.maxValue;
        txtTime.text = TimeUtil.ConvertFloatToString(currentTime);
    }

    public void OutTimeEvent() {
        objQuickTimeEvents.SetActive(false);
    }

    public void UpdateQuickTimeEvent(int currentCakeDone)
    {
        if (sliderQuickTimeEvent.maxValue == 0)
            return;
        txtCountCake.text = currentCakeDone + "/" + sliderQuickTimeEvent.maxValue;
        sliderQuickTimeEvent.value = currentCakeDone;
    }

    public void UpdateTime(float time) {
        txtTime.text = TimeUtil.TimeToString(time, TimeFommat.Keyword);
    }
    #endregion
}
