using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveBase
{
    public bool IsChangeData;
    string stringSave;
    public virtual void SetStringSave(string stringSave) { this.stringSave = stringSave; }
    public virtual void LoadData() { }

    public virtual void IsMarkChangeData()
    {
        IsChangeData = true;
    }

    public virtual void SaveData()
    {
        if (!IsChangeData) return;
        IsChangeData = false;
        PlayerPrefs.SetString(stringSave, JsonUtility.ToJson(this).ToString());
    }

    public string GetJsonData()
    {
        return PlayerPrefs.GetString(stringSave);
    }

    public virtual void Update()
    {
        throw new NotImplementedException();
    }
}
