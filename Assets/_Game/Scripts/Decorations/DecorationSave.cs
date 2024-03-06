using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecorationSave : SaveBase
{
    public List<OwnedDecorationData> ownedDecorationDatas;

    public override void LoadData()
    {
        SetStringSave("DecorationSave");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            DecorationSave data = JsonUtility.FromJson<DecorationSave>(jsonData);
            ownedDecorationDatas = data.ownedDecorationDatas;
        }
        else
        {
            FirstInit();
            IsMarkChangeData();
            SaveData();
        }
    }

    public void FirstInit()
    {
        for (int i = 0; i < 4; i++)
        {
            OwnedDecorationData ownedDecorationData = new OwnedDecorationData();
            ownedDecorationData.decorationType = (DecorationType)(i + 1);
            ownedDecorationData.usingId = 0;
            ownedDecorationData.ownedId = new List<int>();
            ownedDecorationData.ownedId.Add(0);
            ownedDecorationDatas.Add(ownedDecorationData);
        }
    }

    public bool IsOwned(DecorationType type, int id)
    {
        for (int i = 0; i < ownedDecorationDatas.Count; i++)
        {
            if (ownedDecorationDatas[i].decorationType == type)
            {
                return ownedDecorationDatas[i].ownedId.Contains(id);
            }
        }
        return false;
    }

    public bool IsInUse(DecorationType type, int id)
    {
        for (int i = 0; i < ownedDecorationDatas.Count; i++)
        {
            if (ownedDecorationDatas[i].decorationType == type)
            {
                return ownedDecorationDatas[i].usingId == id;
            }
        }
        return false;
    }

    public void UseDecor(DecorationType type, int id)
    {
        for (int i = 0; i < ownedDecorationDatas.Count; i++)
        {
            if (ownedDecorationDatas[i].decorationType == type)
            {
                ownedDecorationDatas[i].usingId = id;
                IsMarkChangeData();
                SaveData();
                break;
            }
        }
    }

    public void BuyDecor(DecorationType type, int id)
    {
        for (int i = 0; i < ownedDecorationDatas.Count; i++)
        {
            if (ownedDecorationDatas[i].decorationType == type)
            {
                ownedDecorationDatas[i].ownedId.Add(id);
                IsMarkChangeData();
                SaveData();
                break;
            }
        }
    }

    public int GetUsingDecor(DecorationType type)
    {
        for (int i = 0; i < ownedDecorationDatas.Count; i++)
        {
            if (ownedDecorationDatas[i].decorationType == type)
            {
                return ownedDecorationDatas[i].usingId;
            }
        }
        return 0;
    }
}
