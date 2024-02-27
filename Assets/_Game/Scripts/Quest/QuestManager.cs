using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public void AddProgress(QuestType qType, float amount) {
        ProfileManager.Instance.playerData.questDataSave.AddProgress(amount, qType);
    }
    public void ClaimQuest(QuestData qData) {
        ProfileManager.Instance.playerData.questDataSave.ClaimQuest(qData);
    }

    public void ClaimSmallQuest(QuestType qType, int id)
    {
        ProfileManager.Instance.playerData.questDataSave.ClaimSmallQuest(qType, id);
    }
}
