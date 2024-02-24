using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorationDataConfig", menuName = "ScriptableObject/DecorationDataConfig")]
public class DecorationDataConfig : ScriptableObject
{
    public List<DecorationDataList> DecorationDataList;

    public float GetDecorPrice(DecorationType type, int id)
    {
        for (int i = 0; i < DecorationDataList.Count; i++)
        {
            if (DecorationDataList[i].decorationType == type)
            {
                for(int j = 0; j < DecorationDataList[i].decorationDatas.Count; j++)
                {
                    if (DecorationDataList[i].decorationDatas[j].id == id)
                        return DecorationDataList[i].decorationDatas[j].price;
                }
            }
        }
        return 0;
    }

    public List<Color> GetDecorColor(DecorationType type, int id)
    {
        for (int i = 0; i < DecorationDataList.Count; i++)
        {
            if (DecorationDataList[i].decorationType == type)
            {
                for (int j = 0; j < DecorationDataList[i].decorationDatas.Count; j++)
                {
                    if (DecorationDataList[i].decorationDatas[j].id == id)
                        return DecorationDataList[i].decorationDatas[j].colors;
                }
            }
        }
        return null;
    }

    public DecorationDataList GetDecorationDataList(DecorationType type)
    {
        for (int i = 0; i < DecorationDataList.Count; i++)
        {
            if (DecorationDataList[i].decorationType == type)
            {
                return DecorationDataList[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class OwnedDecorationData
{
    public DecorationType decorationType;
    public int usingId;
    public List<int> ownedId;
}

[System.Serializable]
public class DecorationDataList
{
    public DecorationType decorationType;
    public List<DecorationData> decorationDatas;
}

[System.Serializable]
public class DecorationData
{
    public int id;
    public Sprite icon;
    public float price;
    public List<Color> colors; // only for table
}

public enum DecorationType
{
    None = 0,
    Table = 1,
    Plate = 2,
    Floor = 3,
    Effect = 4,
}
