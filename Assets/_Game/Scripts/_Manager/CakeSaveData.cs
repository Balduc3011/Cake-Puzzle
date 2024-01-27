
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CakeSaveData : SaveBase
{
    public List<int> cakeIDs = new List<int>();
    public List<int> cakeIDUsing = new List<int>();

    public override void LoadData()
    {
        SetStringSave("CakeSaveData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            CakeSaveData data = JsonUtility.FromJson<CakeSaveData>(jsonData);
            cakeIDs = data.cakeIDs;
            cakeIDUsing = data.cakeIDUsing;
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
    public List<int> cakeIDReturn = new List<int>();
    public List<int> GetCakeIDs(int totalCakeID) {
        for (int i = 0; i < totalCakeID; i++) {
            int randomID = GetRandomCakeID();
            cakeIDReturn[i] = randomID;
        }
        cakeIDReturn.Clear();
        return cakeIDs;

    }

    int GetRandomCakeID()
    {
        int randomIndexX = Random.Range(0, cakeIDs.Count);
        while (cakeIDReturn.Contains(cakeIDs[randomIndexX]))
        {
            randomIndexX = Random.Range(0, cakeIDs.Count);
        }
        return randomIndexX;
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
}
