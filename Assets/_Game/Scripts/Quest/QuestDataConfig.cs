using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataConfig", menuName = "ScriptableObject/QuestDataConfig")]
public class QuestDataConfig : ScriptableObject 
{
    public List<QuestData> questData = new List<QuestData>();
    public List<DailyReward> dailyRewards = new List<DailyReward>();
    string GetNameQuest(QuestData quest) {
        switch (quest.questType)
        {
            case QuestType.WatchADS:
                return Watch_Str + quest.questRequirebase.ToString() + Ads_Str;
            case QuestType.CompleteCake:
                return Bake_Str + quest.questRequirebase.ToString() + Cake_Str;
            
            default:
                return ConstantValue.STR_BLANK;
        }
    }

    private static string Watch_Str = "Watch ";
    private static string Ads_Str = " Ads";
    private static string Bake_Str = "Bake ";
    private static string Cake_Str = " cake";
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