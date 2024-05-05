using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    public int slotIndex;
    [SerializeField] QuestType questType;
    [SerializeField] Button collectBtn;
    [SerializeField] TextMeshProUGUI txtName;
    [SerializeField] TextMeshProUGUI txtProgress;
    [SerializeField] Image rewardIcon;
    [SerializeField] TextMeshProUGUI txtRewardAmount;
    [SerializeField] Slider sProgress;
    [SerializeField] GameObject objSlider;
    float currentProgress;
    float questRequire;

    private void Start()
    {
        collectBtn.onClick.AddListener(CollectQuest);
    }
    public void InitData(QuestType questType)
    {
        this.questType = questType;
        ReInit();
    }

    private void OnEnable()
    {
        ReScale();
        ReInit();
    }

    void ReInit()
    {
        //rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(data.rewardData.ItemType);
        //txtRewardAmount.text = data.rewardData.amount.ToString();
        SetName();
        currentProgress = ProfileManager.Instance.playerData.questDataSave.GetCurrentProgress(questType);
        questRequire = ProfileManager.Instance.playerData.questDataSave.GetCurrentRequire(questType);
        collectBtn.gameObject.SetActive(currentProgress >= questRequire);
        objSlider.SetActive(!collectBtn.gameObject.activeSelf);

        txtProgress.text = currentProgress.ToString() + ConstantValue.STR_SLASH + questRequire;
        if (currentProgress > questRequire)
            currentProgress = questRequire;
        sProgress.maxValue = questRequire;
        sProgress.value = currentProgress;
        
    }

    void SetName()
    {
        switch (questType)
        {
            case QuestType.None:
                break;
            case QuestType.WatchADS:
                txtName.text = "Watch " + 5.ToString() + " ads";
                break;
            case QuestType.CompleteCake:
                txtName.text = "Complete " + 5.ToString() + " cakes";
                break;
            case QuestType.UseBooster:
                txtName.text = "Use " + 5.ToString() + " items";
                break;
            default:
                break;
        }
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
