using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyRewardDataConfig", menuName = "ScriptableObject/DailyRewardDataConfig")]
public class DailyRewardDataConfig : ScriptableObject
{
    public List<DailyRewardConfig> dailyRewardConfig;
}

[System.Serializable]
public class DailyRewardConfig
{
    public int dayIndex;
    public List<DailyRewardData> rewardList;
}

[System.Serializable]
public class DailyRewardData
{
    public DailyItemType dailyItemType;
    public float amount;
}

public enum DailyItemType
{
    Daily1, Daily2, Daily3, Daily4, Daily5, Daily6, Daily7, Daily8, Daily9, Daily10
}