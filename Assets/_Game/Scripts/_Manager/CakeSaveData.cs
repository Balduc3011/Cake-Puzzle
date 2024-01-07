using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CakeSaveData : SaveBase
{
    public List<int> cakeID = new List<int>();

    public override void LoadData()
    {
        SetStringSave("CakeSaveData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            CakeSaveData data = JsonUtility.FromJson<CakeSaveData>(jsonData);
            cakeID = data.cakeID;
        }
        else {
            cakeID.Add(0);
            cakeID.Add(1);
            cakeID.Add(2);
            IsMarkChangeData();
            SaveData();
        }
    }

    public void AddPieces(int piecesID) {
        cakeID.Add(piecesID);
        IsMarkChangeData();
        SaveData();
    }

    public bool IsHaveMoreThanThreeCake()
    {
        return cakeID.Count > 3;
    }
}
