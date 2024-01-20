using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelDataConfig", menuName = "ScriptableObject/LevelDataConfig")]
public class LevelDataConfig : ScriptableObject
{
    public List<LevelData> levelDatas = new List<LevelData>();

    public LevelData GetLevel(int level) {
        return levelDatas[level];
    }
}
[System.Serializable]
public class LevelData {
    public float expUnlock;
    public float cakeUnlockID;
}
