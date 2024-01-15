using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardManager : MonoBehaviour
{
    public bool IsAbleToGetDailyReward(int dayIndex)
    {
        return ProfileManager.Instance.playerData.playerResourseSave.IsAbleToGetDailyReward(dayIndex);
    }
    public bool CheckDailyRewardCollectted(int dayIndex)
    {
        return ProfileManager.Instance.playerData.playerResourseSave.CheckDailyRewardCollectted(dayIndex);
    }

    public void OnGetDailyReward(int dayIndex)
    {
        ProfileManager.Instance.playerData.playerResourseSave.OnGetDailyReward();
        List<ItemData> itemDatas = ProfileManager.Instance.dataConfig.dailyRewardDataConfig.GetDailyRewardList(dayIndex);
        for (int i = 0; i < itemDatas.Count; i++)
        {
            GameManager.Instance.AddItem(itemDatas[i]);
        }
    }
}