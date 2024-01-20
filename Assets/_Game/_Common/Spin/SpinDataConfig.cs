using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinDataConfig", menuName = "ScriptableObject/SpinDataConfig")]
public class SpinDataConfig : ScriptableObject
{
    // Recomend 6 or 8 items
    public List<SpinItemData> spinItemDatas;

    public ItemData GetSpinItemData(int index)
    {
        return spinItemDatas[index].itemData;
    }
}

[System.Serializable]
public class SpinItemData
{
    public ItemData itemData;
    public float rate;
}


public enum SpinItemType
{
    Spin1, Spin2, Spin3, Spin4, Spin5, Spin6, Spin7, Spin8, Spin9, Spin10
}
