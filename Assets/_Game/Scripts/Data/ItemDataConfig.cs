using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataConfig", menuName = "ScriptableObject/ItemDataConfig")]
public class ItemDataConfig : ScriptableObject
{
    public List<ItemDataCF> itemDataCFs = new List<ItemDataCF>();
    public List<ItemType> rewardOnLevelUp = new();
    public List<ItemType> tempRewardList = new();
    public ItemDataCF GetItemData(ItemType itemType) {
        for (int i = 0; i < itemDataCFs.Count; i++)
        {
            if (itemDataCFs[i].itemType == itemType)
            return itemDataCFs[i];
        }
        return null;
    }

    public void InitRewardRandonList()
    {
        tempRewardList = new();
        for (int i = 0; i < rewardOnLevelUp.Count; i++)
        {
            tempRewardList.Add(rewardOnLevelUp[i]);
        }
    }

    public void RemoveFromTemp(ItemType itemType)
    {
        for (int i = tempRewardList.Count - 1; i >= 0; i--)
        {
            if (tempRewardList[i] == itemType)
            {
                tempRewardList.RemoveAt(i);
            }
        }
    }

    public ItemType GetRewardItemOnLevel()
    {
        return tempRewardList[Random.Range(0, tempRewardList.Count)];
    }

    public ItemType GetRewardItem()
    {
        return rewardOnLevelUp[Random.Range(0, rewardOnLevelUp.Count)];
    }
}

[System.Serializable]
public class ItemDataCF {
    public string title;
    public ItemType itemType;
    public string description;
}