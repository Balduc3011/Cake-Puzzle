using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CakeDataConfig", menuName = "ScriptableObject/CakeDataConfig")]
public class CakeDataConfig : ScriptableObject
{
    public List<CakeData> cakeDatas;
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
}

[System.Serializable]
public class CakeData
{
    public int id;
    public Sprite icon;
    public Mesh pieceMesh;
    public Mesh cakeMesh;
}
