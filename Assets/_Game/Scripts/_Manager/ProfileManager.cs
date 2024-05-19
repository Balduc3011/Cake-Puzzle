using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class ProfileManager : Singleton<ProfileManager>
{
    public VersionStatus versionStatus;
    public PlayerData playerData;
    public DataConfig dataConfig;
    protected override void Awake()
    {
        base.Awake();
        playerData.LoadData();
        DOTween.SetTweensCapacity(3000, 200);
    }
    private void Update()
    {
        playerData.Update();
    }

    public bool IsShowCheat()
    {
        return (versionStatus == VersionStatus.Cheat);
    }

    public bool IsShowDebug()
    {
        return (versionStatus is VersionStatus.Cheat or VersionStatus.NoCheat);
    }

    public bool GetSettingStatus(SettingId settingId)
    {
        return playerData.playerResourseSave.GetSettingStatus(settingId).status;
    }

    public void ChangeSettingStatus(SettingId settingId)
    {
        playerData.playerResourseSave.ChangeSettingStatus(settingId);
    }
}
