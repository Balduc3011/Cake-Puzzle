using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSpin : UIPanel
{
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    public SheetAnimation sheetAnimation;
    Transform Transform;
    [SerializeField] Button closeBtn;
    [SerializeField] Button spinBtn;
    [SerializeField] Button stopBtn;
    [SerializeField] GameObject adsSpinObj;
    [SerializeField] RectTransform dynamicSpinWheel;
    [SerializeField] List<SpinSlide> slides;
    [SerializeField] bool spin;
    [SerializeField] bool stopClicked;
    [SerializeField] float stopCounter;
    float stopCooldow = 2.5f;
    [SerializeField] GameObject blocker;

    [SerializeField] SpinState spinState;
    float defaultSpinSpeed = 25f;
    float maxSpinSpeed = 600f;
    [SerializeField] float spinSpeed;
    [SerializeField] float acceleration;
    float stopAfter = 2; //Run these wheel time before completely stop
    float accelerationTime = 2.5f;
    float firstMark = 45f / 2f;
    float markValue = 45f;
    float overrun = 0;
    [SerializeField] int selectedSlide = -1;
    float amountToSpin;
    float spinTime = 4;
    public override void Awake()
    {
        panelType = UIPanelType.PanelSpin;
        base.Awake();
        Transform = transform;
    }

    private void OnEnable()
    {
        spinBtn.gameObject.SetActive(true);
        stopBtn.gameObject.SetActive(false);
        spinSpeed = defaultSpinSpeed;
        spinState = SpinState.Default;
        //spin = true;
        CheckFreeSpin();
        Transform.SetAsLastSibling();
        stopClicked = false;
        stopCounter = stopCooldow;
        dynamicSpinWheel.eulerAngles = Vector3.zero;
        sheetAnimation.PlayAnim();
        StopRewardSlide();
    }

    void CheckFreeSpin()
    {
        adsSpinObj.SetActive(!GameManager.Instance.spinManager.IsHasFreeSpin());
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClose);
        spinBtn.onClick.AddListener(OnSpinClick);
        //stopBtn.onClick.AddListener(OnStopSpin);
        Init();
        acceleration = (maxSpinSpeed - defaultSpinSpeed) / accelerationTime;
        markValue = 360 / slides.Count;
        firstMark = markValue / 2f;
    }

    void Init()
    {
        List<SpinItemData> spinItemDatas = ProfileManager.Instance.dataConfig.spinDataConfig.spinItemDatas;
        for (int i = 0; i < spinItemDatas.Count; i++)
        {
            slides[i].Init(i, spinItemDatas[i]);
        }
    }

    void OnClose()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        //openAndCloseAnim.OnClose(CloseInstant);
        uiPanelShowUp.OnClose(CloseInstant);
    }

    void CloseInstant()
    {
        UIManager.instance.ClosePanelSpin();
    }

    void StopRewardSlide()
    {
        for (int i = 0; i < slides.Count; i++)
        {
            slides[i].OnReward(false);
        }
    }

    void OnSpinClick()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        if(GameManager.Instance.spinManager.IsHasFreeSpin())
            OnSpin();
        else
        {
            if (GameManager.Instance.IsHasNoAds())
                OnSpin();
            else
                GameManager.Instance.ShowRewardVideo(WatchVideoRewardType.FreeSpinAds, OnSpin);
        }
    }

    void OnSpin()
    {
        slides[selectedSlide].OnReward(false);
        spinState = SpinState.Spin;
        spin = true;
        spinBtn.gameObject.SetActive(false);
        //stopBtn.gameObject.SetActive(false);
        selectedSlide = GameManager.Instance.spinManager.OnSpin();
        stopClicked = false;
        stopCounter = stopCooldow;
        blocker.SetActive(true);
        amountToSpin = (360 * 4) + ((slides.Count - selectedSlide) * markValue);
        dynamicSpinWheel.DORotate(Vector3.back * amountToSpin, spinTime, RotateMode.FastBeyond360).OnComplete(OnSpinStoped).SetEase(Ease.OutQuart);
    }

    void OnSpinStoped()
    {
        spinBtn.gameObject.SetActive(true);
        CheckFreeSpin();
        CheckResult();
    }

    void CheckResult()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_LevelUp);
        blocker.SetActive(false);
        GameManager.Instance.spinManager.OnSpinStoped();
    }
}

public enum SpinState
{
    Default,
    Spin,
    Stop,
    WaitToStop
}
