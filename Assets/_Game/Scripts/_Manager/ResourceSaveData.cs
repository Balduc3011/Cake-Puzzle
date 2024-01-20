using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSaveData : SaveBase
{
    public int level;
    public float currentExp;
    public override void LoadData()
    {
        SetStringSave("ResourceSaveData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            ResourceSaveData data = JsonUtility.FromJson<ResourceSaveData>(jsonData);
            level = data.level;
            currentExp = data.currentExp;
        }
        else { 
            level = 0;
            currentExp = 0;
            IsMarkChangeData();
            SaveData();
        }
    }

    void LevelUp() { 
        level++;
        if (level > ProfileManager.Instance.dataConfig.levelDataConfig.levelDatas.Count)
        {
            level = ProfileManager.Instance.dataConfig.levelDataConfig.levelDatas.Count;
        }
        else { 
            currentExp = 0;
        }
        IsMarkChangeData();
        SaveData();
    }
    public void ChangeCurrentExp(float expAdd) {
        currentExp += expAdd;
        if (currentExp >= ProfileManager.Instance.dataConfig.levelDataConfig.GetLevel(level).expUnlock) {
            LevelUp();
            return; 
        }
        IsMarkChangeData();
        SaveData();
    }
}
