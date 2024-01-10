using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinManager : MonoBehaviour
{
    ChanceTable<int> chanceTable = new ChanceTable<int>();
    int selectedItemId = -1;
    void Start()
    {
        SetUpTable();
    }

    void SetUpTable()
    {
        List<SpinItemData> spinItemDatas = ProfileManager.Instance.dataConfig.spinDataConfig.spinItemDatas;
        for (int i = 0; i < spinItemDatas.Count; i++)
        {
            chanceTable.AddItem(i, (int)spinItemDatas[i].rate);
        }
    }

    public int OnGetSelectedItem()
    {
        selectedItemId = chanceTable.GetRandomItem();
        return selectedItemId;
    }
}
