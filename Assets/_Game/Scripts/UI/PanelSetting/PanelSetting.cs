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
        UIManager.instance.ShowPanelLoading();
        OnClose();
    }
}
