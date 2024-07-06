using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CakeDataConfig", menuName = "ScriptableObject/CakeDataConfig")]
public class CakeDataConfig : ScriptableObject
{
    public List<CakeData> cakeDatas;
    public List<CakeLevelData> cakeLevelDatas;
    public List<CakeObjectByLevel> cakeObjectByLevels;

    //private void OnEnable()
    //{
    //    InitCardLevelData();
    //}

    public Sprite GetCakeIcon(int cakeID, int cakeTier) {
        for (int i = 0; i < cakeDatas.Count; i++)
        {
            if (cakeDatas[i].id == cakeID)
            return cakeDatas[i].icons[cakeTier];
        }
        return null;
    }

    public CakeData GetCakeData(int id)
    {
        for (int i = 0; i < cakeDatas.Count; i++)
        {
            if (cakeDatas[i].id == id)
            { return cakeDatas[i]; }
        }
        return null;
    }

    public Mesh GetCakePieceMesh(int id, int level = 1)
    {
        for (int i = 0; i < cakeDatas.Count; i++)
        {
            if (cakeDatas[i].id == id)
            { return cakeDatas[i].pieces[level - 1]; }
        }
        return null;
    }

    public int GetRandomCake()
    {
        return cakeDatas[Random.Range(0, cakeDatas.Count)].id;
    }

    void InitCardLevelData()
    {
        cakeLevelDatas.Clear();
        for (int i = 0; i < 100; i++)
        {
            CakeLevelData cakeLevelData = new();
            cakeLevelData.level = i + 1;
            cakeLevelData.cardRequire = i == 0 ? 1 : i * 5;
            cakeLevelDatas.Add(cakeLevelData);
        }
    }

    public int GetCardAmountToLevelUp(int level)
    {
        for (int i = 0; i < cakeLevelDatas.Count; i++)
        {
            if (cakeLevelDatas[i].level == level)
                return cakeLevelDatas[i].cardRequire;
        }
        return 0;
    }

    public GameObject GetCakePref(int cakeId)
    {
        int level = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(cakeId);
        if(level > 2) level = 2;
        for (int i = 0; i < cakeObjectByLevels.Count; i++)
        {
            if (cakeObjectByLevels[i].id == cakeId) 
                return cakeObjectByLevels[i].GetCakePref(level);
        }
        return null;
    }

    public Mesh GetCakePieceMesh2(int cakeId)
    {
        int levelPref = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(cakeId);
        if (levelPref > 2) levelPref = 2;
        for (int i = 0; i < cakeDatas.Count; i++)
        {
            if (cakeDatas[i].id == cakeId)
            {
                return cakeDatas[i].pieces[levelPref - 1];
            }
        }
        return null;
    }
}

[System.Serializable]
public class CakeData
{
    public int id;
    public List<Sprite> icons;
    public List<Mesh> pieces;
    public List<Mesh> fullCake;
}

[System.Serializable] 
public class CakeLevelData
{
    public int level;
    public int cardRequire;
}

[System.Serializable]
public class CakeObjectByLevel
{
    public int id;
    public List<GameObject> cakePref;
    public List<Sprite> cakeIcon;
    public GameObject GetCakePref(int level)
    {
        return cakePref[level - 1];
    }
}
