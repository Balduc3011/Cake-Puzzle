using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    public int slotIndex;
    QuestType questType;
    [SerializeField] Button collectBtn;
    [SerializeField] TextMeshProUGUI txtName;
    [SerializeField] TextMeshProUGUI txtProgress;
    [SerializeField] Image rewardIcon;
    [SerializeField] TextMeshProUGUI txtRewardAmount;
    [SerializeField] Slider sProgress;
    [SerializeField] GameObject objHighlight;
    [SerializeField] GameObject objHide;
    [SerializeField] GameObject objSlider;
    [SerializeField] GameObject objUnable;
    float currentProgress;
    float questRequire;

    private void Start()
    {
        collectBtn.onClick.AddListener(CollectQuest);
    }
    public void InitData(QuestType questType)
    {
        this.questType = questType;
    }

    private void OnEnable()
    {
        ReScale();
    }

    void ReInit()
    {
        //txtName.text = data.questName.ToString();
        //rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(data.rewardData.ItemType);
        //txtRewardAmount.text = data.rewardData.amount.ToString();
        //questRequire = data.questRequirebase;

        //currentProgress = ProfileManager.Instance.playerData.questDataSave.GetCurrentProgress(data.questType, id);
        //if (currentProgress > questRequire)
        //    currentProgress = questRequire;
        //txtProgress.text = currentProgress.ToString() + ConstantValue.STR_SLASH + questRequire;
        //sProgress.maxValue = questRequire;
        //sProgress.value = currentProgress;
        //bool isClaimed = ProfileManager.Instance.playerData.questDataSave.IsClaimQuest(data.questType, id);
        //collectBtn.interactable = currentProgress == questRequire && !isClaimed;
        //collectBtn.gameObject.SetActive(collectBtn.interactable);
        //objSlider.SetActive(!collectBtn.interactable);
        //objUnable.SetActive(!collectBtn.interactable);
        //objHighlight.SetActive(collectBtn.interactable);
        //objHide.SetActive(currentProgress == questRequire && isClaimed);
    }

    public void ReScale()
    {
        transform.DOScale(1, 0.15f).From(0).SetDelay(0.25f + 0.1f * slotIndex);
    }

    void CollectQuest()
    {
        GameManager.Instance.questManager.ClaimQuest(questType);
        ReInit();
        ReScale();
    }
}
