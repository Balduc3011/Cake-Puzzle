using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : SlotBase<QuestData>
{
    int id;
    [SerializeField] TextMeshProUGUI txtName;
    [SerializeField] TextMeshProUGUI txtProgress;
    [SerializeField] Image rewardIcon;
    [SerializeField] TextMeshProUGUI txtRewardAmount;
    [SerializeField] Slider sProgress;
    [SerializeField] GameObject objHighlight;
    [SerializeField] GameObject objHide;
    float currentProgress;
    float questRequire;
    public override void InitData(QuestData data)
    {
        base.InitData(data);
        id = data.id;
        txtName.text = data.questName.ToString();
        rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(data.rewardData.ItemType);
        txtRewardAmount.text = data.rewardData.amount.ToString();
        questRequire = data.questRequirebase;

        currentProgress = ProfileManager.Instance.playerData.questDataSave.GetCurrentProgress(data.questType, id);
        if (currentProgress > questRequire)
            currentProgress = questRequire;
        txtProgress.text = currentProgress.ToString() + ConstantValue.STR_SLASH + questRequire;
        sProgress.maxValue = questRequire; 
        sProgress.value = currentProgress;
        bool isClaimed = ProfileManager.Instance.playerData.questDataSave.IsClaimQuest(data.questType, id);
        btnChoose.interactable = currentProgress == questRequire && !isClaimed;
        objHighlight.SetActive(btnChoose.interactable);
        objHide.SetActive(currentProgress == questRequire && isClaimed);
        if(isClaimed)
        {
            transform.SetAsLastSibling();
        }
    }

    private void OnEnable()
    {
        if(data != null)
        {
            currentProgress = ProfileManager.Instance.playerData.questDataSave.GetCurrentProgress(data.questType, id);
            if (currentProgress > questRequire)
                currentProgress = questRequire;
            txtProgress.text = currentProgress.ToString() + ConstantValue.STR_SLASH + questRequire;
            sProgress.value = currentProgress;

            //bool isClaimed = ProfileManager.Instance.playerData.questDataSave.IsClaimQuest(data.questType, id);
            //btnChoose.interactable = currentProgress == questRequire && !isClaimed;
            //objHighlight.SetActive(btnChoose.interactable);
            //objHide.SetActive(currentProgress == questRequire && isClaimed);
            //if (isClaimed)
            //{
            //    transform.SetAsLastSibling();
            //}
        }
    }

    public void CheckCollect()
    {
        bool isClaimed = ProfileManager.Instance.playerData.questDataSave.IsClaimQuest(data.questType, id);
        btnChoose.interactable = currentProgress == questRequire && !isClaimed;
        objHighlight.SetActive(btnChoose.interactable);
        objHide.SetActive(currentProgress == questRequire && isClaimed);
        if (isClaimed)
        {
            transform.SetAsLastSibling();
        }
    }

    public override void OnChoose()
    {
        base.OnChoose();
        GameManager.Instance.questManager.ClaimSmallQuest(data.questType, id);
        List<ItemData> rewards = new List<ItemData>();
        rewards.Add(data.rewardData);
        GameManager.Instance.GetItemReward(rewards);
        btnChoose.interactable = false;
        objHighlight.SetActive(false);
        objHide.SetActive(true);
        transform.SetAsLastSibling();
        UIManager.instance.ShowPanelItemsReward();
    }
}
