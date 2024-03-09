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
    }

    void CheckFreeSpin()
    {
        adsSpinObj.SetActive(!GameManager.Instance.spinManager.IsHasFreeSpin());
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClose);
        spinBtn.onClick.AddListener(OnSpin);
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
        //openAndCloseAnim.OnClose(CloseInstant);
        uiPanelShowUp.OnClose(CloseInstant);
    }

    void CloseInstant()
    {
        UIManager.instance.ClosePanelSpin();
    }

    void OnSpin()
    {
        spinState = SpinState.Spin;
        spin = true;
        spinBtn.gameObject.SetActive(false);
        //stopBtn.gameObject.SetActive(false);
        selectedSlide = GameManager.Instance.spinManager.OnSpin();
        stopClicked = false;
        stopCounter = stopCooldow;
        blocker.SetActive(true);
    }

    void OnSpinToMax()
    {
        spinBtn.gameObject.SetActive(false);
        //stopBtn.gameObject.SetActive(true);
    }

    void OnStopSpin()
    {
        spinBtn.gameObject.SetActive(false);
        //stopBtn.gameObject.SetActive(false);
        if (selectedSlide >= 0)
        {
            spinState = SpinState.WaitToStop;
        }
        else
        {
            spinState = SpinState.Stop;
            overrun = 0;
        }
    }

    bool CheckWaitToStop()
    {
        float stopAngle = dynamicSpinWheel.eulerAngles.z;
        stopAngle = stopAngle % 360;
        if(selectedSlide == 0)
        {
            return stopAngle > 360 - 15f ||
                stopAngle < +15f;
        }
        else
        {
            return stopAngle > firstMark + (selectedSlide - 1) * markValue - 15f  &&
                stopAngle < firstMark + (selectedSlide - 1) * markValue + 15f;
        }
    }

    void OnStopAtDirectSlide()
    {
        spinState = SpinState.Stop;
        overrun = 0;
    }

    void OnSpinStoped()
    {
        spinState = SpinState.Default;
        spinSpeed = 0;
        spin = false;
        spinBtn.gameObject.SetActive(true);
        CheckFreeSpin();
        //stopBtn.gameObject.SetActive(false);
        CheckResult();
    }

    void CheckResult()
    {
        blocker.SetActive(false);
        float stopAngle = dynamicSpinWheel.eulerAngles.z;
        stopAngle = stopAngle % 360;
        int index = 0;
        if(stopAngle > firstMark + (slides.Count - 1) * markValue)
        {
            index = 0;
        }
        else
        {
            while (firstMark + index * markValue < stopAngle)
            {
                index++;
            }
        }
        GameManager.Instance.spinManager.OnSpinStoped();
    }

    private void Update()
    {
        if (spin)
        {
            if(!stopClicked)
            {
                stopCounter -= Time.deltaTime;
                if(stopCounter <= 0)
                {
                    OnStopSpin();
                    stopClicked = true;
                }
            }
            switch (spinState)
            {
                case SpinState.Default:
                    spinSpeed = defaultSpinSpeed;
                    break;
                case SpinState.Spin:
                    if (spinSpeed < maxSpinSpeed)
                    {
                        spinSpeed += Time.deltaTime * acceleration;
                        if (spinSpeed >= maxSpinSpeed)
                        {
                            OnSpinToMax();
                        }
                    }    
                    break;
                case SpinState.Stop:
                    if (overrun < stopAfter * 360 - 1)
                    {
                        spinSpeed = maxSpinSpeed * (stopAfter * 360 - overrun) / (stopAfter * 360);
                    }
                    else
                    {
                        spinSpeed = 0;
                        OnSpinStoped();
                    }
                    break;
                case SpinState.WaitToStop:
                    if(CheckWaitToStop())
                    {
                        OnStopAtDirectSlide();
                    }
                    break;
                default:
                    break;
            }
            dynamicSpinWheel.eulerAngles -= new Vector3(0f, 0f, Time.deltaTime * spinSpeed);
            overrun += Time.deltaTime * spinSpeed;
        }
    }
}

public enum SpinState
{
    Default,
    Spin,
    Stop,
    WaitToStop
}
