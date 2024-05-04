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
    public List<QuestProcess> quessProcess = new List<QuestProcess>();
    DateTime dateTimeOnQuest;
    public override void LoadData()
    {
        SetStringSave("QuestDataSave");
        string json = GetJsonData();
        if (!string.IsNullOrEmpty(json))
        {
            QuestDataSave data = JsonUtility.FromJson<QuestDataSave>(json);
            quessProcess = data.quessProcess;
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
        quessProcess.Clear();
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
        if (quessProcess == null)
        {
            quessProcess = new List<QuestProcess>();
            for (int i = 0; i < 3; i++)
            {
                QuestProcess quest = new QuestProcess();
                quest.questType = (QuestType)(i + 1);
                quessProcess.Add(quest);
            }
        }
        for (int i = 0; i < quessProcess.Count; i++)
        {
            quessProcess[i].marked = 0;
            quessProcess[i].process = 0;
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
        //for (int i = 0; i < quessProcess.Count; i++)
        //{
        //    if (quessProcess[i].questType == questType && quessProcess[i].id == id)
        //    {
        //        return quessProcess[i].progress;
        //    }
        //}
        return 0;
    }

    public void ClaimQuest(QuestType questType) {
        // TODO
        IsMarkChangeData();
        SaveData();
        EventManager.TriggerEvent(EventName.ChangeStarDailyQuest.ToString());
    }

    public void AddProgress(float amount, QuestType questType) {
        for (int i = 0; i < quessProcess.Count; i++)
        {
            if (quessProcess[i].questType == questType)
            {
                quessProcess[i].process += amount;
            }
        }
        IsMarkChangeData();
        SaveData();
    }

    public bool CheckShowNoticeQuest() {
        return false;
    }
}

