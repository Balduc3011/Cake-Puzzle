using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class QuestDataSave : SaveBase
{
    public string timeOnQuest;
    public int starsEarned;
    public int rewardEarned;
    public List<QuestSave> questSaves = new List<QuestSave>();
    QuestSave questTemp;
    DateTime dateTimeOnQuest;
    public override void LoadData()
    {
        SetStringSave("QuestDataSave");
        string json = GetJsonData();
        if (!string.IsNullOrEmpty(json))
        {
            QuestDataSave data = JsonUtility.FromJson<QuestDataSave>(json);
            questSaves = data.questSaves;
            timeOnQuest = data.timeOnQuest;
            starsEarned = data.starsEarned;
            rewardEarned = data.rewardEarned;
            InitData();
        }
        else {
            ResetTime();
        }
    }

    void ResetTime() {
        timeOnQuest = DateTime.Now.ToString();
        questSaves.Clear();
        starsEarned = 0;
        rewardEarned = 0;
        InitQuest();
        IsMarkChangeData();
        SaveData();
    }

    public void InitData()
    {
        DateTime.TryParse(timeOnQuest, out dateTimeOnQuest);
        if (DateTime.Now.Day > dateTimeOnQuest.Day)
            ResetTime();
    }

    void InitQuest()
    {
        List<QuestData> questData = ProfileManager.Instance.dataConfig.questDataConfig.questData;
        for (int i = 0; i < questData.Count; i++)
        {
            QuestSave quest = new QuestSave();
            quest.id = i;
            quest.questType = questData[i].questType;
            quest.progress = 0;
            quest.earned = false;
            questSaves.Add(quest);
        }
    }

    TimeSpan timeReturn;

    public double GetTimeCoolDown() {
        DateTime timeEndDay = DateTime.Today.AddDays(1);
        timeReturn = timeEndDay.Subtract(DateTime.Now);
        if (timeReturn > TimeSpan.Zero)
            return timeReturn.TotalSeconds;
        else
            return 0;
    }

    public int GetStarEarned() { return starsEarned; }

    public void GetReward() {
        rewardEarned++;
        IsMarkChangeData();
        SaveData();
    }

    public bool CheckCanEarnQuest(int pointIndex) {
        return pointIndex > rewardEarned;
    }

    public float GetCurrentProgress(QuestType questType, int id) {
        //questTemp = questSaves.Find(e => e.questType == questType);
        //if (questTemp == null) return 0;
        //else return questTemp.progress;
        for (int i = 0; i < questSaves.Count; i++)
        {
            if (questSaves[i].questType == questType && questSaves[i].id == id)
            {
                return questSaves[i].progress;
            }
        }
        return 0;
    }

    public void ClaimQuest(QuestData questData) {
        starsEarned += questData.questStarEarn;
        questTemp = questSaves.Find(e => e.questType == questData.questType);
        questTemp.earned = true;
        IsMarkChangeData();
        SaveData();
        EventManager.TriggerEvent(EventName.ChangeStarDailyQuest.ToString());
    }

    public void ClaimSmallQuest(QuestType questType, int id)
    {
        //starsEarned += questData.questStarEarn;
        //questTemp = questSaves.Find(e => e.questType == questData.questType);
        //questTemp.earned = true;
        for (int i = 0; i < questSaves.Count; i++)
        {
            if (questSaves[i].questType == questType && questSaves[i].id == id)
            {
                questSaves[i].earned = true;
            }
        }
        IsMarkChangeData();
        SaveData();
        EventManager.TriggerEvent(EventName.ChangeStarDailyQuest.ToString());
    }

    public bool IsClaimQuest(QuestType questType, int id) {
        //questTemp = questSaves.Find(e => e.questType == questType);
        //if (questTemp == null) return true;
        //return questTemp.earned;
        for (int i = 0; i < questSaves.Count; i++)
        {
            if (questSaves[i].questType == questType && questSaves[i].id == id)
            {
                return questSaves[i].earned;
            }
        }
        return false;
    }

    public void AddProgress(float amount, QuestType questType) {
        //questTemp = questSaves.Find(e => e.questType == questType);
        //if (questTemp == null)
        //{
        //    QuestSave newQuestSave = new QuestSave();
        //    newQuestSave.questType = questType;
        //    newQuestSave.progress = amount;
        //    questSaves.Add(newQuestSave);
        //}
        //else questTemp.progress += amount;
        Debug.Log(questType);
        for (int i = 0; i < questSaves.Count; i++)
        {
            if (questSaves[i].questType == questType)
            {
                questSaves[i].progress += amount;
            }
        }
        IsMarkChangeData();
        SaveData();
    }

    public bool CheckShowNoticeQuest() {
        questTemp = questSaves.Find(e => e.earned == false);
        return questTemp != null;
    }
}

[System.Serializable]
public class QuestSave {
    public int id;
    public QuestType questType;
    public float progress;
    public bool earned;
}
