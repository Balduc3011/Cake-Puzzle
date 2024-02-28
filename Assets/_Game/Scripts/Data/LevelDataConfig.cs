using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelDataConfig", menuName = "ScriptableObject/LevelDataConfig")]
public class LevelDataConfig : ScriptableObject
{
    public List<LevelData> levelDatas = new List<LevelData>();

    public LevelData GetLevel(int level) {
        if (level >= levelDatas.Count)
            return levelDatas[levelDatas.Count - 1];
        return levelDatas[level];
    }

    public float GetExpToNextLevel(int currentLevel)
    {
        if (currentLevel >= levelDatas.Count)
            return levelDatas[levelDatas.Count - 1].expUnlock * currentLevel - levelDatas.Count + 1;
        return levelDatas[currentLevel].expUnlock;
    }

    public int GetLevelMax()
    {
        return levelDatas.Count - 1;
    }

    public int GetCakeID(int currentLevel)
    {
        if (currentLevel >= levelDatas.Count)
            return -1;
        return levelDatas[currentLevel].cakeUnlockID;
    }
}
[System.Serializable]
public class LevelData {
    public float expUnlock;
    public int cakeUnlockID;
}
