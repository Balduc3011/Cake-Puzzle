
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
    public List<CakeOnWait> cakeOnWaits = new List<CakeOnWait>();
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
            cakeOnWaits = data.cakeOnWaits;
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
            if (c.plateIndex.indexX == plate.indexX && c.plateIndex.indexY == plate.indexY)
            {
                if (cake == null || cake.pieces.Count == 0 || cake.pieces.Count == 6)
                {
                    RemoveCake(plate);
                }
                else { 
                    c.SetCakeID(cake);
                    IsMarkChangeData();
                    SaveData();
                }
                return;
            }
        }
        if (cake == null || cake.pieces.Count == 0 || cake.pieces.Count == 6)
        {
            return;
        }
        CakeOnPlate newCakeOnPlate = new CakeOnPlate();
        newCakeOnPlate.plateIndex = plate;
        newCakeOnPlate.SetCakeID(cake);
        cakeOnPlates.Add(newCakeOnPlate);
        IsMarkChangeData();
        SaveData();
    }

    public void RemoveCake(PlateIndex plate) {
        foreach (CakeOnPlate c in cakeOnPlates)
        {
            if (c.plateIndex.indexX == plate.indexX && c.plateIndex.indexY == plate.indexY)
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
        cakeOnWaits.Clear();
        IsMarkChangeData();
        SaveData();
    }


    public void AddCakeWait(GroupCake groupCake, int cakeIndex) {
        if (cakeIndex >= cakeOnWaits.Count)
        {
            CakeOnWait newCakeOnWait = new CakeOnWait();
            cakeOnWaits.Add(newCakeOnWait);
        }
        for (int i = 0; i < groupCake.cake.Count; i++)
        {
            if (cakeOnWaits[cakeIndex] == null)
                cakeOnWaits[cakeIndex] = new CakeOnWait();
            cakeOnWaits[cakeIndex].SaveCake(i, groupCake.cake[i]);
        }
        IsMarkChangeData();
        SaveData();
    }
    public void RemoveCakeWait(int cakeIndex) {
        cakeOnWaits[cakeIndex] = null;
    }

    public bool IsHaveCakeWaitSave()
    {
        for (int i = 0; i < cakeOnWaits.Count; i++)
        {
            if (cakeOnWaits[i] != null)
                return true;
        }
        return false;
    }

    public bool IsHaveCakeOnPlate()
    {
        return cakeOnPlates.Count > 0;
    }
}

[System.Serializable]
public class CakeOnPlate {
    public PlateIndex plateIndex;
    public List<int> cakeIDs = new List<int>();
    public void SetCakeID(Cake cake) {
        cakeIDs.Clear();
        for (int i = 0; i < cake.pieces.Count; i++)
        {
            cakeIDs.Add(cake.pieces[i].cakeID);
        }
    }
}

[System.Serializable]
public class CakeOnWait {
    public List<CakeSave> cakeSaves = new List<CakeSave>();
    public void SaveCake(int cakeIndex, Cake cake) {
        if (cakeIndex >= cakeSaves.Count)
        {
            CakeSave cakeSave = new CakeSave();
            cakeSave.SetCakeID(cake);
            cakeSaves.Add(cakeSave);
            return;
        }

        cakeSaves[cakeIndex].SetCakeID(cake);
    }
}

[System.Serializable]
public class CakeSave {
    public List<int> pieceCakeIDCount = new List<int>();
    public List<int> pieceCakeID = new List<int>();
    public void SetCakeID(Cake cake)
    {
        pieceCakeIDCount = cake.pieceCakeIDCount;
        pieceCakeID = cake.pieceCakeID;
    }
}
