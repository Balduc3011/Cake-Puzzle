using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataConfig", menuName = "ScriptableObject/ItemDataConfig")]
public class ItemDataConfig : ScriptableObject
{
    public List<ItemDataCF> itemDataCFs = new List<ItemDataCF>();
    public ItemDataCF GetItemData(ItemType itemType) {
        for (int i = 0; i < itemDataCFs.Count; i++)
        {
            if (itemDataCFs[i].itemType == itemType)
            return itemDataCFs[i];
        }
        return null;
    }
}

[System.Serializable]
public class ItemDataCF {
    public string title;
    public ItemType itemType;
    public string description;
}
