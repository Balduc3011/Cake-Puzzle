using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    List<ItemData> defaultQuestReward;
    void InitReward()
    {
        defaultQuestReward = new List<ItemData>();
        ItemData itemData = new ItemData();
        defaultQuestReward.Add(itemData);
        itemData.ItemType = Random.Range(-1f, 3f) > 0 ? ItemType.Coin : ItemType.Cake;
        if(itemData.ItemType == ItemType.Coin)
        {
            itemData.amount = ConstantValue.VAL_QUEST_COIN;
            itemData.subId = -1;
        }
        else
        {
            itemData.amount = (int)(Random.Range(5, 10));
            itemData.subId = ProfileManager.Instance.playerData.cakeSaveData.GetRandomOwnedCake();
        }
            
    }

    public void AddProgress(QuestType qType, float amount) {
        ProfileManager.Instance.playerData.questDataSave.AddProgress(amount, qType);
    }
    public void ClaimQuest(QuestType questType) {
        InitReward();
        ProfileManager.Instance.playerData.questDataSave.ClaimQuest(questType);
        GameManager.Instance.GetItemRewards(defaultQuestReward);
        UIManager.instance.ShowPanelItemsReward();
    }
}
