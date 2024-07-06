using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public SaveBase saveBase;
    public CakeSaveData cakeSaveData;
    public PlayerResourseSave playerResourseSave;
    public DecorationSave decorationSave;
    public QuestDataSave questDataSave;
    

    public void LoadData()
    {
        playerResourseSave.LoadData();
        cakeSaveData.LoadData();
        saveBase.LoadData();
        decorationSave.LoadData();
        questDataSave.LoadData();
    }

    public void SaveData()
    {
        saveBase.SaveData();
    }

    public void Update()
    {
        saveBase.Update();
    }
}
