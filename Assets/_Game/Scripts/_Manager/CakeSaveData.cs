
using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CakeSaveData : SaveBase
{
    public List<OwnedCake> ownedCakes = new();
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
            //Debug.Log(jsonData);
            CakeSaveData data = JsonUtility.FromJson<CakeSaveData>(jsonData);
            ownedCakes = data.ownedCakes;
            cakeIDs = data.cakeIDs;
            cakeIDUsing = data.cakeIDUsing;
            cakeOnPlates = data.cakeOnPlates;
            cakeOnWaits = data.cakeOnWaits;
            UpdateCardRequire();
        }
        else {
            AddFirstCake();
            UpdateCardRequire();
            IsMarkChangeData();
            SaveData();
        }
        if (ProfileManager.Instance.playerData.playerResourseSave.currentLevel == 0)
        {
            AddTutorialCake();
            IsMarkChangeData();
            SaveData();
        }
    }

    void AddFirstCake()
    {
        AddCakeCard(0, 1);
        AddCakeCard(1, 1);

        cakeIDs.Add(0);
        cakeIDs.Add(1);

        cakeIDUsing.Add(0);
        cakeIDUsing.Add(1);
    }

    public OwnedCake GetOwnedCake(int cakeId)
    {
        for (int i = 0; i < ownedCakes.Count; i++)
        {
            if (ownedCakes[i].cakeID == cakeId)
            {
                ownedCakes[i].UpdateCardRequire();
                return ownedCakes[i];
            }
        }
        return null;
    }

    public int GetOwnedCakeLevel(int cakeId)
    {
        for (int i = 0; i < ownedCakes.Count; i++)
        {
            if (ownedCakes[i].cakeID == cakeId)
            {
                return ownedCakes[i].level;
            }
        }
        return 1;
    }

    void UpdateCardRequire()
    {
        for (int i = 0; i < ownedCakes.Count; i++)
        {
            ownedCakes[i].UpdateCardRequire();
        }
    }

    public int GetRandomUnlockedCake()
    {
        return cakeIDs[UnityEngine.Random.Range(0, cakeIDs.Count)];
    }

    public void AddCake(int cakeId) 
    {
        cakeIDs.Add(cakeId);
        IsMarkChangeData();
        SaveData();
    }

    public void AddCakeCard(int cakeId, int amount)
    {
        if (cakeId == -1) return;
        for (int i = 0; i < ownedCakes.Count; i++)
        {
            if (ownedCakes[i].cakeID == cakeId)
            {
                ownedCakes[i].AddCard(amount);
                IsMarkChangeData();
                SaveData();
                return;
            }
        }
        OwnedCake cake = new();
        cake.cakeID = cakeId;
        cake.level = 1;
        cake.cardAmount = amount;
        ownedCakes.Add(cake);
        IsMarkChangeData();
        SaveData();
    }

    public int GetRandomOwnedCake()
    {
        return ownedCakes[UnityEngine.Random.Range(0, ownedCakes.Count)].cakeID;
    }

    public void OnUpgradeCard(OwnedCake ownedCake)
    {
        ownedCake.OnUpgradeCard();
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

    public int GetCakeIdByIndex(int index)
    {
        if(index < cakeIDUsing.Count)
        {
            return cakeIDUsing[index];
        }
        return 0;
    } 
    public bool IsUsingCake(int cake)
    {
        return cakeIDUsing.Contains(cake);
    }

    public void UseCake(int cake)
    {
        if (cakeIDUsing.Count >= 6 || !IsOwnedCake(cake))
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

    public void SwapCake(int oldId, int newId)
    {
        for (int i = 0; i < cakeIDUsing.Count; i++)
        {
            if (cakeIDUsing[i] == oldId)
                cakeIDUsing[i] = newId;
        }
        for (int i = 0; i < cakeOnPlates.Count; i++)
        {
            for (int j = 0; j < cakeOnPlates[i].cakeIDs.Count; j++)
            {
                if(cakeOnPlates[i].cakeIDs[j] == oldId)
                {
                    cakeOnPlates[i].cakeIDs[j] = newId;
                }
            }
        }
        for (int i = 0; i < cakeOnWaits.Count; i++)
        {
            for (int j = 0; j < cakeOnWaits[i].cakeSaves.Count; j++)
            {
                for (int k = 0; k < cakeOnWaits[i].cakeSaves[j].pieceCakeID.Count; k++)
                {
                    if (cakeOnWaits[i].cakeSaves[j].pieceCakeID[k] == oldId)
                    {
                        cakeOnWaits[i].cakeSaves[j].pieceCakeID[k] = newId;
                    }
                }
            }
        }
        IsMarkChangeData();
        SaveData();
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
        EventManager.TriggerEvent(EventName.UpdateCakeOnPlate.ToString());
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

    public bool HasCakeUpgradeable()
    {
        for (int i = 0; i < ownedCakes.Count; i++)
        {
            if (ownedCakes[i].IsAbleToUpgrade())
                return true;
        }
        return false;
    }

    void AddTutorialCake()
    {
        ClearAllCake();

        CakeOnWait cakeOnWait1 = new CakeOnWait();
        cakeOnWait1.cakeSaves = new List<CakeSave>();
        CakeSave cakeSave1 = new CakeSave();
        cakeOnWait1.cakeSaves.Add(cakeSave1);
        cakeSave1.pieceCakeIDCount = new List<int>();
        cakeSave1.pieceCakeIDCount.Add(3);
        cakeSave1.pieceCakeID = new List<int>();
        cakeSave1.pieceCakeID.Add(0);
        cakeOnWaits.Add(cakeOnWait1);

        CakeOnWait cakeOnWait2 = new CakeOnWait();
        cakeOnWait2.cakeSaves = new List<CakeSave>();
        CakeSave cakeSave2 = new CakeSave();
        cakeOnWait2.cakeSaves.Add(cakeSave2);
        cakeSave2.pieceCakeIDCount = new List<int>();
        cakeSave2.pieceCakeIDCount.Add(3);
        cakeSave2.pieceCakeID = new List<int>();
        cakeSave2.pieceCakeID.Add(1);
        cakeOnWaits.Add(cakeOnWait2);

        CakeOnWait cakeOnWait3 = new CakeOnWait();
        cakeOnWait3.cakeSaves = new List<CakeSave>();
        CakeSave cakeSave3 = new CakeSave();
        cakeOnWait3.cakeSaves.Add(cakeSave3);
        cakeSave3.pieceCakeIDCount = new List<int>();
        cakeSave3.pieceCakeIDCount.Add(3);
        cakeSave3.pieceCakeID = new List<int>();
        cakeSave3.pieceCakeID.Add(0);
        cakeOnWaits.Add(cakeOnWait3);
    }
    public void SetData(int cakeID, int currentTier)
    {
        for (int i = 0; i < ownedCakes.Count; i++)
        {
            if (ownedCakes[i].cakeID == cakeID)
            {
                ownedCakes[i].level = currentTier + 1;
                IsMarkChangeData();
                SaveData();
                return;
            }
        }
        OwnedCake cake = new();
        cake.cakeID = cakeID;
        cake.level = currentTier + 1;
        ownedCakes.Add(cake);
        IsMarkChangeData();
        SaveData();
    }
    public int GetCakeIDForMission() {
        return cakeIDUsing[UnityEngine.Random.Range(0, cakeIDUsing.Count - 1)];
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

[System.Serializable]
public class OwnedCake
{
    public int cakeID;
    public int level;
    public int cardAmount;
    int cardRequire;

    public void AddCard(int amount)
    {
        cardAmount += amount;
        //if(cardAmount >= cardRequire)
        //{
        //    level++;
        //    cardAmount -= cardRequire;
        //    UpdateCardRequire();
        //}
        EventManager.TriggerEvent(EventName.AddCakeCard.ToString());
    }

    public void OnUpgradeCard()
    {
        if (cardAmount >= cardRequire)
        {
            level++;
            ABIAnalyticsManager.Instance.TrackEventCakeLevelUp(level, cakeID);
            cardAmount -= cardRequire;
            UpdateCardRequire();
            GameManager.Instance.cakeManager.ReInitData(cakeID);
        }

    }

    public void UpdateCardRequire()
    {
        cardRequire = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCardAmountToLevelUp(level + 1);
    }

    public int CardRequire { get => cardRequire; }

    public bool IsAbleToUpgrade()
    {
        UpdateCardRequire();
        float upgradePrice = upgradePrice = ConstantValue.VAL_CAKEUPGRADE_COIN * level;
        return cardAmount >= cardRequire &&
                ProfileManager.Instance.playerData.playerResourseSave.IsHasEnoughMoney(upgradePrice);
    }
}
