using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

[System.Serializable]
public class PlayerResourseSave : SaveBase
{
    public BigNumber coins;
    public List<ItemData> ownedItem;
    public int trophy;
    public int trophyRecord;
    public float piggySave;
    public string lastFreeSpin;
    public string lastDay;
    public string x2BoosterEnd;
    public int dailyRewardedDay;

    public int currentLevel;
    public float currentExp;
    public List<SettingValue> settingValues;

    int levelMax;
    float expMax;
    public override void LoadData()
    {
        SetStringSave("PlayerResourseSave");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            PlayerResourseSave data = JsonUtility.FromJson<PlayerResourseSave>(jsonData);
            ownedItem = data.ownedItem;
            coins = data.coins;
            trophy = data.trophy;
            trophyRecord = data.trophyRecord;
            piggySave = data.piggySave;
            lastFreeSpin = data.lastFreeSpin;
            lastDay = data.lastDay;
            x2BoosterEnd = data.x2BoosterEnd;
            dailyRewardedDay = data.dailyRewardedDay;
            currentLevel = data.currentLevel;
            currentExp = data.currentExp;
            settingValues = data.settingValues;
            CheckDay();
        }
        else
        {
            SetNewSetting();
            IsMarkChangeData();
            SaveData();
        }
        levelMax = ProfileManager.Instance.dataConfig.levelDataConfig.GetLevelMax();
        expMax = ProfileManager.Instance.dataConfig.levelDataConfig.GetExpToNextLevel(currentLevel);
    }
    void SetNewSetting()
    {
        for (int i = 0; i < 4; i++)
        {
            SettingValue setting = new SettingValue();
            setting.settingId = (SettingId)i;
            setting.status = true;
            settingValues.Add(setting);
        }
    }

    void CheckDay()
    {
        if (!String.IsNullOrEmpty(lastDay))
        {
            DateTime lastDay = DateTime.Parse(this.lastDay);
            if(lastDay.Date != DateTime.Now.Date)
            {
                if (dailyRewardedDay >= 7) dailyRewardedDay = 0;
            }
        }
    }

    public bool IsHasEnoughMoney(float amount)
    {
        return coins.IsBigger(amount);
    }

    public void AddMoney(float amount)
    {
        coins.Add(amount);
        IsMarkChangeData();
        SaveData();
        EventManager.TriggerEvent(EventName.ChangeCoin.ToString());
    }

    public void ConsumeMoney(float amount)
    {
        coins.Substract(amount);
        IsMarkChangeData();
        SaveData();
        EventManager.TriggerEvent(EventName.ChangeCoin.ToString());
    }

    public void AddTrophy(int amount)
    {
        trophy += amount;
        IsMarkChangeData();
        SaveData();
    }

    public void SaveRecord()
    {
        trophyRecord = trophy;
        IsMarkChangeData();
        SaveData();
    }

    public void AddPiggySave(float amount = 10)
    {
        piggySave += amount;
        if (piggySave >= ConstantValue.VAL_MAX_PIGGY)
            piggySave = ConstantValue.VAL_MAX_PIGGY;
        IsMarkChangeData();
        SaveData();
    }

    public void ClearPiggySave()
    {
        piggySave = 0;
        IsMarkChangeData();
        SaveData();
    }

    public bool IsHasFreeSpin()
    {
        if (!String.IsNullOrEmpty(lastFreeSpin))
        {
            DateTime lastSpinTime = DateTime.Parse(lastFreeSpin);
            TimeSpan span = (DateTime.Now).Subtract(lastSpinTime);
            //return (span.Days > 0);
            return (DateTime.Now.Day - lastSpinTime.Day >= 1);
        }
        return true;
    }

    public void OnSpin()
    {
        if(IsHasFreeSpin())
        {
            lastFreeSpin = DateTime.Now.ToString();
            IsMarkChangeData();
            SaveData();
        }
        UIManager.instance.panelTotal.CheckNoti();
    }

    public bool IsHasDailyReward()
    {
        if (!String.IsNullOrEmpty(lastDay))
        {
            DateTime lastDay = DateTime.Parse(this.lastDay);
            return lastDay.Date != DateTime.Now.Date;
        }
        else
        {
            return true;
        }
    }

    public bool IsAbleToGetDailyReward(int dayIndex)
    {
        if (!String.IsNullOrEmpty(lastDay))
        {
            DateTime lastDay = DateTime.Parse(this.lastDay);
            return dayIndex == dailyRewardedDay &&
                        lastDay.Date != DateTime.Now.Date;
        }
        else
        {
            return dayIndex == dailyRewardedDay;
        }
    }

    public bool CheckDailyRewardCollectted(int dayIndex)
    {
        if (!String.IsNullOrEmpty(lastDay))
        {
            return dayIndex < dailyRewardedDay;
        }
        else
        {
            return false;
        }
    }

    public void OnGetDailyReward()
    {
        dailyRewardedDay++;
        lastDay = DateTime.Now.ToString();
        IsMarkChangeData();
        SaveData();
        UIManager.instance.panelTotal.CheckNoti();
    }

    public void AddItem(ItemData item)
    {
        if(item.ItemType == ItemType.Coin)
        {
            AddMoney(item.amount);
        }
        for (int i = 0; i < ownedItem.Count; i++)
        {
            if (ownedItem[i].ItemType == item.ItemType) {
                ownedItem[i].amount += item.amount;
                IsMarkChangeData();
                SaveData();
                return;
            }
        }
        ItemData data = new ItemData();
        data.ItemType = item.ItemType;
        data.amount = item.amount;
        ownedItem.Add(data);
        IsMarkChangeData();
        SaveData();
    }

    public float GetItemAmount(ItemType itemType)
    {
        for (int i = 0; i < ownedItem.Count; i++)
        {
            if (ownedItem[i].ItemType == itemType)
            {
                return ownedItem[i].amount;
            }
        }
        return 0;
    }

    public void UsingItem(ItemType itemType)
    {
        for (int i = 0; i < ownedItem.Count; i++)
        {
            if (ownedItem[i].ItemType == itemType)
            {
                Debug.Log("using item : "+itemType);
                ownedItem[i].amount--;
                return;
            }
        }
        IsMarkChangeData();
        SaveData();
    }

    public void AddExp(float expAdd) {
        if (currentLevel >= levelMax && currentExp==expMax) {
            LevelUp();
            return;
        }
        currentExp += expAdd;
        if (currentExp >= expMax)
        {
            currentExp = 0;
            LevelUp();
        }
        IsMarkChangeData();
        SaveData();
    }

    public void LevelUp() {
        //int cakeID = ProfileManager.Instance.dataConfig.levelDataConfig.GetCakeID(currentLevel);
        //if (cakeID != -1)
        //{
        //    ProfileManager.Instance.playerData.cakeSaveData.AddCake(cakeID);
        //    ProfileManager.Instance.playerData.cakeSaveData.UseCake(cakeID);
        //}
        currentLevel++;
        GameManager.Instance.GetLevelUpReward();
        EventManager.TriggerEvent(EventName.ChangeLevel.ToString());
        expMax = ProfileManager.Instance.dataConfig.levelDataConfig.GetExpToNextLevel(currentLevel);
    }

    public string GetCurrentExp()
    {
        return currentExp + "/" + expMax;
    }
    public float GetMaxExp()
    {
        return expMax;
    }

    public bool IsHaveItem(ItemType itemType)
    {
        for (int i = 0; i < ownedItem.Count; i++)
        {
            if (ownedItem[i].ItemType == itemType && ownedItem[i].amount > 0)
                return true;
        }
        return false;
    }

    public void OnGetX2Booster()
    {
        x2BoosterEnd = DateTime.Now.AddMinutes((double)ConstantValue.VAL_X2BOOSTER_TIME).ToString();
        IsMarkChangeData();
        SaveData();
    }

    public float GetX2BoosterRemain()
    {
        if (!String.IsNullOrEmpty(x2BoosterEnd))
        {
            DateTime lastSpinTime = DateTime.Parse(x2BoosterEnd);
            TimeSpan span = lastSpinTime.Subtract(DateTime.Now);
            return span.TotalSeconds > 0 ? (float)span.TotalSeconds : 0f;
        }
        return 0f;
    }

    public bool HasX2Booster()
    {
        if (!String.IsNullOrEmpty(x2BoosterEnd))
        {
            DateTime lastSpinTime = DateTime.Parse(x2BoosterEnd);
            TimeSpan span = lastSpinTime.Subtract(DateTime.Now);
            return span.TotalSeconds > 0;
        }
        return false;
    }
    public SettingValue GetSettingStatus(SettingId settingId)
    {
        for (int i = 0; i < settingValues.Count; i++)
        {
            if (settingValues[i].settingId == settingId)
            {
                return settingValues[i];
            }
        }
        SettingValue setting = new SettingValue();
        setting.settingId = settingId;
        setting.status = true;
        settingValues.Add(setting);
        return setting;
    }

    public void ChangeSettingStatus(SettingId settingId)
    {
        SettingValue setting = GetSettingStatus(settingId);
        setting.status = !setting.status;
        IsMarkChangeData();
        SaveData();
    }
}

[System.Serializable]
public class SettingValue
{
    public SettingId settingId;
    public bool status;
}
public enum SettingId
{
    None = 0,
    Music = 1,
    Sound = 2,
    Vibrate = 3,
}
