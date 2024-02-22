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
    public string lastDay;
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
            lastDay = data.lastDay;
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
                if (!String.IsNullOrEmpty(lastDay))
                {
                    return dayIndex == dailyRewardedDay &&
                        DateTime.Parse(lastDay).Date != DateTime.Now.Date;
                }
                return true;   
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
        lastDay = DateTime.Now.ToString();
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

    public void UsingItem(ItemType itemType)
    {
        for (int i = 0; i < ownedItem.Count; i++)
        {
            if (ownedItem[i].ItemType == itemType)
            {
                ownedItem[i].amount--;
                return;
            }
        }
        IsMarkChangeData();
        SaveData();
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
            int cakeID = ProfileManager.Instance.dataConfig.levelDataConfig.GetCakeID(currentLevel);
            if (cakeID != -1)
            {
                ProfileManager.Instance.playerData.cakeSaveData.AddCake(cakeID);
                ProfileManager.Instance.playerData.cakeSaveData.UseCake(cakeID);
            }
            currentLevel++;
            EventManager.TriggerEvent(EventName.ChangeLevel.ToString());
            expMax = ProfileManager.Instance.dataConfig.levelDataConfig.GetExpToNextLevel(currentLevel);
        }
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
            if (ownedItem[i].ItemType == itemType && ownedItem.Count > 0)
                return true;
        }
        return false;
    }
}
