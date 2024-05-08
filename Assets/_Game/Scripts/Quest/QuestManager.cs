using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    List<ItemData> defaultQuestReward;
    private void Start()
    {
        defaultQuestReward = new List<ItemData>();
        ItemData itemData = new ItemData();
        itemData.ItemType = ItemType.Coin;
        itemData.amount = ConstantValue.VAL_QUEST_COIN;
        defaultQuestReward.Add(itemData);
    }
    public void AddProgress(QuestType qType, float amount) {
        ProfileManager.Instance.playerData.questDataSave.AddProgress(amount, qType);
    }
    public void ClaimQuest(QuestType questType) {
        ProfileManager.Instance.playerData.questDataSave.ClaimQuest(questType);
        GameManager.Instance.GetItemRewards(defaultQuestReward);
        UIManager.instance.ShowPanelItemsReward();
    }

}
