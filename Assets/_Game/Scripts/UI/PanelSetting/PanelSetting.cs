using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSetting : UIPanel
{
    Transform Transform;
    [SerializeField] Button closeBtn;
    [SerializeField] Button toMenuBtn;
    [SerializeField] SwitchButton musicSwitch;
    [SerializeField] SwitchButton soundSwitch;
    [SerializeField] GameObject settingNoti;
    public override void Awake()
    {
        panelType = UIPanelType.PanelSetting;
        base.Awake();
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClose);
        toMenuBtn.onClick.AddListener(BackToMenu);
        Transform = transform;
        SetFirstState();
        SetUpCheat();
    }

    void SetFirstState()
    {
        musicSwitch.SetActive(ProfileManager.Instance.GetSettingStatus(SettingId.Music), true);
        soundSwitch.SetActive(ProfileManager.Instance.GetSettingStatus(SettingId.Sound), true);

        musicSwitch.SetUp(() => {
            ProfileManager.Instance.ChangeSettingStatus(SettingId.Music);
            musicSwitch.SetActive(ProfileManager.Instance.GetSettingStatus(SettingId.Music));
            GameManager.Instance.audioManager.ChangeMusicState(ProfileManager.Instance.GetSettingStatus(SettingId.Music));
        });
        soundSwitch.SetUp(() => {
            ProfileManager.Instance.ChangeSettingStatus(SettingId.Sound);
            soundSwitch.SetActive(ProfileManager.Instance.GetSettingStatus(SettingId.Sound));
            GameManager.Instance.audioManager.ChangeSoundState(ProfileManager.Instance.GetSettingStatus(SettingId.Sound));
        });
    }

    private void OnEnable()
    {
        toMenuBtn.gameObject.SetActive(GameManager.Instance.playing);
        if(Transform == null) Transform = transform;
        Transform.SetAsLastSibling();
        settingNoti.SetActive(ProfileManager.Instance.playerData.cakeSaveData.HasCakeUpgradeable());
    }

    void OnClose()
    {
        openAndCloseAnim.OnClose(UIManager.instance.ClosePanelSetting);
    }

    void BackToMenu()
    {
        GameManager.Instance.BackToMenu();
        OnClose();
    }

    [SerializeField] GameObject cheatObj;
    [SerializeField] Button coinBtn;
    [SerializeField] Button boosterBtn;

    void SetUpCheat()
    {
        cheatObj.SetActive(ProfileManager.Instance.versionStatus == VersionStatus.Cheat);
        coinBtn.onClick.AddListener(AddCoin);
        boosterBtn.onClick.AddListener(AddBooster);
    }

    void AddCoin()
    {
        ItemData coins = new ItemData();
        coins.ItemType = ItemType.Coin;
        coins.amount = 2000;
        GameManager.Instance.AddItem(coins);
        EventManager.TriggerEvent(EventName.AddItem.ToString());
    }

    void AddBooster()
    {
        ItemData b1 = new ItemData();
        b1.ItemType = ItemType.Hammer;
        b1.amount = 20;
        GameManager.Instance.AddItem(b1);
        ItemData b2 = new ItemData();
        b2.ItemType = ItemType.FillUp;
        b2.amount = 20;
        GameManager.Instance.AddItem(b2);
        ItemData b3 = new ItemData();
        b3.ItemType = ItemType.ReRoll;
        b3.amount = 20;
        GameManager.Instance.AddItem(b3);
        EventManager.TriggerEvent(EventName.AddItem.ToString());
    }
}
