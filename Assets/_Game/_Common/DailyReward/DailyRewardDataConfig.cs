using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyRewardDataConfig", menuName = "ScriptableObject/DailyRewardDataConfig")]
public class DailyRewardDataConfig : ScriptableObject
{
    public List<DailyRewardConfig> dailyRewardConfig;
    public List<ItemData> GetDailyRewardList(int dayIndex)
    {
        return dailyRewardConfig[dayIndex].rewardList;
    }
}

[System.Serializable]
public class DailyRewardConfig
{
    public int dayIndex;
    public List<ItemData> rewardList;
}