using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class QuestDataSave : SaveBase
{
    public string timeOnQuest;
    public float starsEarned;
    public List<int> rewardEarned;
    public List<QuestProcess> quessProcess = new List<QuestProcess>();
    DateTime dateTimeOnQuest;
    List<QuestTargetData> questTargetDatas;
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
        starsEarned = 0;
        rewardEarned = new List<int>();
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
        if (quessProcess == null || quessProcess.Count == 0)
        {
            quessProcess.Clear();
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

    public void GetReward(int id) {
        rewardEarned.Add(id);
        IsMarkChangeData();
        SaveData();
        UIManager.instance.panelTotal.CheckNoti();
    }

    public bool CheckCanEarnQuest(int id, float star) {
        return starsEarned >= star &&
            !rewardEarned.Contains(id);
    }

    public float GetCurrentProgress(QuestType questType) {
        for (int i = 0; i < quessProcess.Count; i++)
        {
            if (quessProcess[i].questType == questType)
            {
                return quessProcess[i].process;
            }
        }
        QuestProcess quest = new QuestProcess();
        quest.questType = questType;
        quessProcess.Add(quest);
        return 0;
    }
    
    public float GetCurrentRequire(QuestType questType) {
        for (int i = 0; i < quessProcess.Count; i++)
        {
            if (quessProcess[i].questType == questType)
            {
                return ProfileManager.Instance.dataConfig.questDataConfig.GetQuestRequire(questType, quessProcess[i].marked);
            }
        }
        QuestProcess quest = new QuestProcess();
        quest.questType = questType;
        quessProcess.Add(quest);
        return ProfileManager.Instance.dataConfig.questDataConfig.GetQuestRequire(questType, 0);
    }

    public void ClaimQuest(QuestType questType) {
        // TODO
        bool found = false;
        for (int i = 0; i < quessProcess.Count; i++)
        {
            if (quessProcess[i].questType == questType)
            {
                quessProcess[i].marked++;
                found = true;
            }
        }
        if (!found)
        {
            QuestProcess quest = new QuestProcess();
            quest.questType = questType;
            quest.marked = 1;
            quessProcess.Add(quest);
        }
        starsEarned += ConstantValue.VAL_QUEST_STAR;
        IsMarkChangeData();
        SaveData();
        EventManager.TriggerEvent(EventName.ChangeStarDailyQuest.ToString());
        UIManager.instance.panelTotal.CheckNoti();
    }

    public void AddProgress(float amount, QuestType questType) {
        bool found = false;
        for (int i = 0; i < quessProcess.Count; i++)
        {
            if (quessProcess[i].questType == questType)
            {
                quessProcess[i].process += amount;
                found = true;
            }
        }
        if(!found)
        {
            QuestProcess quest = new QuestProcess();
            quest.questType = questType;
            quest.process = amount;
            quessProcess.Add(quest);
        }
        IsMarkChangeData();
        SaveData();
        UIManager.instance.panelTotal.CheckNoti();
    }

    public bool CheckShowNoticeQuest() {
        for (int i = 0; i < quessProcess.Count; i++)
        {
            if(GetCurrentProgress(quessProcess[i].questType) > GetCurrentRequire(quessProcess[i].questType))
            {
                return true;
            }
        }
        if(questTargetDatas == null) questTargetDatas = ProfileManager.Instance.dataConfig.questDataConfig.questTargetDatas;
        for (int i = 0; i < questTargetDatas.Count; i++)
        {
            if (CheckCanEarnQuest(i, questTargetDatas[i].require)) return true;
        }
        return false;
    }
}

