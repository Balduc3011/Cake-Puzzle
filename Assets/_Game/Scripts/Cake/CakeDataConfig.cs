using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CakeDataConfig", menuName = "ScriptableObject/CakeDataConfig")]
public class CakeDataConfig : ScriptableObject
{
    public List<CakeData> cakeDatas;
    public List<CakeLevelData> cakeLevelDatas;

    //private void OnEnable()
    //{
    //    InitCardLevelData();
    //}
    public CakeData GetCakeData(int id)
    {
        for (int i = 0; i < cakeDatas.Count; i++)
        {
            if (cakeDatas[i].id == id)
            { return cakeDatas[i]; }
        }
        return null;
    }

    public Mesh GetCakePieceMesh(int id)
    {
        for (int i = 0; i < cakeDatas.Count; i++)
        {
            if (cakeDatas[i].id == id)
            { return cakeDatas[i].pieceMesh; }
        }
        return null;
    }
    public Mesh GetCakeMesh(int id)
    {
        for (int i = 0; i < cakeDatas.Count; i++)
        {
            if (cakeDatas[i].id == id)
            { return cakeDatas[i].cakeMesh; }
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
}

[System.Serializable]
public class CakeData
{
    public int id;
    public Sprite icon;
    public Mesh pieceMesh;
    public Mesh cakeMesh;
}

[System.Serializable] 
public class CakeLevelData
{
    public int level;
    public int cardRequire;
}
