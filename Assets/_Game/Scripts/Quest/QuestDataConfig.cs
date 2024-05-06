using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataConfig", menuName = "ScriptableObject/QuestDataConfig")]
public class QuestDataConfig : ScriptableObject 
{
    public List<QuestData> questData = new List<QuestData>();
    public List<DailyReward> dailyRewards = new List<DailyReward>();

    public float GetQuestRequire(QuestType type, int mark)
    {
        for (int i = 0; i < questData.Count; i++)
        {
            if (questData[i].questType == type)
            {
                return questData[i].questRequirebase + mark * questData[i].step;
            }
        }
        return 0;
    }
}

[System.Serializable]
public class QuestProcess
{
    public QuestType questType;
    public float process;
    public int marked;
}

[System.Serializable]
public class QuestData {
    public int questRequirebase;
    public QuestType questType;
    public ItemData rewardData;
    public int step;
}

[System.Serializable]
public class DailyReward {
    public ItemData rewardData;
    public int pointGet;
}

public enum QuestType
{
    None = 0,
    WatchADS = 1,
    CompleteCake = 2,
    UseBooster = 3
}