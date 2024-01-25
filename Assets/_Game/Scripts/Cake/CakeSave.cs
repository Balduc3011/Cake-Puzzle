using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CakeSave : SaveBase
{
    public List<int> ownedCakes;
    public List<int> usingCake;
    public override void LoadData()
    {
        SetStringSave("CakeSave");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            CakeSave data = JsonUtility.FromJson<CakeSave>(jsonData);
            ownedCakes = data.ownedCakes;
            usingCake = data.usingCake;
        }
        else
        {
            ownedCakes = new List<int>();
            ownedCakes.Add(0);
            usingCake = new List<int>();
            usingCake.Add(0);
            IsMarkChangeData();
            SaveData();
        }
    }

    public bool IsOwnedCake(int cake)
    {
        return ownedCakes.Contains(cake);
    }
    public bool IsUsingCake(int cake)
    {
        return usingCake.Contains(cake);
    }

    public void AddCake(int cake)
    {
        if (!ownedCakes.Contains(cake))
            ownedCakes.Add(cake);
        IsMarkChangeData();
        SaveData();
    }

    public void UseCake(int cake)
    {
        if(!usingCake.Contains(cake))
            usingCake.Add(cake);
        IsMarkChangeData();
        SaveData();
    }

    public void RemoveUsingCake(int cake)
    {
        if (usingCake.Contains(cake))
            usingCake.Remove(cake);
        IsMarkChangeData();
        SaveData();
    }
}
