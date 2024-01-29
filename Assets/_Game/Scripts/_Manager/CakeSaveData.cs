
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CakeSaveData : SaveBase
{
    public List<int> cakeIDs = new List<int>();
    public List<int> cakeIDUsing = new List<int>();
    public List<CakeOnPlate> cakeOnPlates = new List<CakeOnPlate>();
    public override void LoadData()
    {
        SetStringSave("CakeSaveData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            CakeSaveData data = JsonUtility.FromJson<CakeSaveData>(jsonData);
            cakeIDs = data.cakeIDs;
            cakeIDUsing = data.cakeIDUsing;
            cakeOnPlates = data.cakeOnPlates;
        }
        else {
            cakeIDs.Add(0);
            cakeIDs.Add(1);

            cakeIDUsing.Add(0);
            cakeIDUsing.Add(1);
            IsMarkChangeData();
            SaveData();
        }
    }

    public void AddCake(int cakeId) {
        cakeIDs.Add(cakeId);
        IsMarkChangeData();
        SaveData();
    }

    public bool IsHaveMoreThanThreeCake()
    {
        return cakeIDs.Count > 3;
    }

    public bool IsOwnedCake(int cake)
    {
        return cakeIDs.Contains(cake);
    }

    public bool IsUsingCake(int cake)
    {
        return cakeIDUsing.Contains(cake);
    }

    public void UseCake(int cake)
    {
        if (cakeIDUsing.Count >= 5 || !IsOwnedCake(cake))
            return;
        if (!cakeIDUsing.Contains(cake))
        {
            cakeIDUsing.Add(cake);
            IsMarkChangeData();
            SaveData();
        }
    }

    public void RemoveUsingCake(int cake)
    {
        if (cakeIDUsing.Contains(cake))
        {
            cakeIDUsing.Remove(cake);
            IsMarkChangeData();
            SaveData();
        }
    }

    public void SaveCake(PlateIndex plate, Cake cake)
    {
        foreach (CakeOnPlate c in cakeOnPlates) {
            if (c.plateIndex == plate)
            {
                c.SetCakeID(cake.pieces);
                IsMarkChangeData();
                SaveData();
                return;
            }
        }
        CakeOnPlate newCakeOnPlate = new CakeOnPlate();
        newCakeOnPlate.plateIndex = plate;
        newCakeOnPlate.SetCakeID(cake.pieces);
        cakeOnPlates.Add(newCakeOnPlate);
        IsMarkChangeData();
        SaveData();
    }

    public void RemoveCake(PlateIndex plate) {
        foreach (CakeOnPlate c in cakeOnPlates)
        {
            if (c.plateIndex == plate)
            {
                cakeOnPlates.Remove(c);
                IsMarkChangeData();
                SaveData();
                return;
            }
        }
    }

    public void ClearAllCake()
    {
        cakeOnPlates.Clear();
        IsMarkChangeData();
        SaveData();
    }
}

[System.Serializable]
public class CakeOnPlate {
    public PlateIndex plateIndex;
    public List<int> cakeIDs = new List<int>();
    public void SetCakeID(List<Piece> pieces) {
        cakeIDs.Clear();
        for (int i = 0; i < pieces.Count; i++) { cakeIDs.Add(pieces[i].cakeID); }
    }
}
