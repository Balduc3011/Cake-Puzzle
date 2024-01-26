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
    public string lastFreeSpin;
    public string firstDay;
    public int dailyRewardedDay;

    public int currentLevel;
    public float currentExp;

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
            lastFreeSpin = data.lastFreeSpin;
            firstDay = data.firstDay;
            dailyRewardedDay = data.dailyRewardedDay;
            currentLevel = data.currentLevel;
            currentExp = data.currentExp;
        }
        else
        {
            firstDay = DateTime.Now.ToString();
            IsMarkChangeData();
            SaveData();
        }
        levelMax = ProfileManager.Instance.dataConfig.levelDataConfig.GetLevelMax();
        expMax = ProfileManager.Instance.dataConfig.levelDataConfig.GetExpToNextLevel(currentLevel);
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
    }

    public bool IsAbleToGetDailyReward(int dayIndex)
    {
        if (!String.IsNullOrEmpty(firstDay))
        {
            DateTime firstDayTime = DateTime.Parse(firstDay);
            TimeSpan span = (DateTime.Now).Subtract(firstDayTime);
            //if (span.Days >= dayIndex)
            if (DateTime.Now.Day - firstDayTime.Day >= dayIndex)
            {
                return dayIndex == dailyRewardedDay;
            }
            else return false;

        }
        else
        {
            firstDay = DateTime.Now.ToString();
            return dayIndex == dailyRewardedDay;
        }
    }

    public bool CheckDailyRewardCollectted(int dayIndex)
    {
        if (!String.IsNullOrEmpty(firstDay))
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
        IsMarkChangeData();
        SaveData();
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

    public void AddExp(float expAdd) {
        if (currentLevel >= levelMax && currentExp==expMax) { return; }
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
        if (currentLevel < levelMax)
        {
            currentLevel++;
            EventManager.TriggerEvent(EventName.ChangeLevel.ToString());
            expMax = ProfileManager.Instance.dataConfig.levelDataConfig.GetExpToNextLevel(currentLevel);
            int cakeID = ProfileManager.Instance.dataConfig.levelDataConfig.GetCakeID(currentLevel);
            if (cakeID != -1)
            {
                ProfileManager.Instance.playerData.cakeSaveData.AddCake(cakeID);
            }
        }
    }

    public string GetCurrentExp()
    {
        return currentExp + "/" + expMax;
    }

    public string GetCurrentLevel()
    {
        return currentLevel.ToString();
    }

    public float GetMaxExp()
    {
        return expMax;
    }
}
