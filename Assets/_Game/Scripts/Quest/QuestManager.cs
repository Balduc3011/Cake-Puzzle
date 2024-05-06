using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public void AddProgress(QuestType qType, float amount) {
        ProfileManager.Instance.playerData.questDataSave.AddProgress(amount, qType);
    }
    public void ClaimQuest(QuestType questType) {
        ProfileManager.Instance.playerData.questDataSave.ClaimQuest(questType);
    }
}
