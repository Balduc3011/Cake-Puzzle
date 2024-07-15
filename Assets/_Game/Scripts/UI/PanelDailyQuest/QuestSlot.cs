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
    [SerializeField] TextMeshProUGUI txtRewardAmount;
    [SerializeField] Image rewardIcon;
    [SerializeField] Slider sProgress;
    [SerializeField] GameObject objSlider;
    float currentProgress;
    float questRequire;
    List<ItemData> defaultQuestReward;

    private void Start()
    {
        collectBtn.onClick.AddListener(CollectQuest);
        InitReward();
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
        InitRewardShow();
    }

    void ReInit()
    {
        currentProgress = ProfileManager.Instance.playerData.questDataSave.GetCurrentProgress(questType);
        questRequire = ProfileManager.Instance.playerData.questDataSave.GetCurrentRequire(questType);
        collectBtn.interactable = (currentProgress >= questRequire);

        txtProgress.text = currentProgress.ToString() + ConstantValue.STR_SLASH + questRequire;
        if (currentProgress > questRequire)
            currentProgress = questRequire;
        sProgress.maxValue = questRequire;
        sProgress.value = currentProgress;
        SetName();
    }

    void SetName()
    {
        switch (questType)
        {
            case QuestType.None:
                break;
            case QuestType.WatchADS:
                txtName.text = "Watch " + questRequire.ToString() + " ads";
                break;
            case QuestType.CompleteCake:
                txtName.text = "Complete " + questRequire.ToString() + " cakes";
                break;
            case QuestType.UseBooster:
                txtName.text = "Use " + questRequire.ToString() + " items";
                break;
            default:
                break;
        }
    }

    public void ReScale()
    {
        transform.DOScale(1, 0.25f).From(0).SetDelay(0.25f + 0.1f * slotIndex);
    }

    void CollectQuest()
    {
        GameManager.Instance.questManager.ClaimQuest(questType);
        ReInit();
        ReScale();
        GameManager.Instance.GetItemRewards(defaultQuestReward);
        UIManager.instance.ShowPanelItemsReward();
        InitReward();
    }

    void InitReward()
    {
        defaultQuestReward = new List<ItemData>();
        ItemData itemData = new ItemData();
        defaultQuestReward.Add(itemData);
        itemData.ItemType = Random.Range(-1f, 1.5f) > 0 ? ItemType.Coin : ItemType.Cake;
        if (itemData.ItemType == ItemType.Coin)
        {
            itemData.amount = ConstantValue.VAL_QUEST_COIN;
            itemData.subId = -1;
            rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(ItemType.Coin);
        }
        else
        {
            itemData.amount = (int)(Random.Range(1, 6));
            for (int i = 0; i < Random.Range(1, 6); i++)
            {
                itemData.subId = ProfileManager.Instance.playerData.cakeSaveData.GetRandomOwnedCake();
            }
            rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetCakeSprite(itemData.subId);
        }
        txtRewardAmount.text = itemData.amount.ToString();
    }

    void InitRewardShow()
    {
        if (defaultQuestReward == null || defaultQuestReward.Count == 0) return;
        if (defaultQuestReward[0].ItemType == ItemType.Coin)
        {
            rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(ItemType.Coin);
        }
        else
        {
            rewardIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetCakeSprite(defaultQuestReward[0].subId);
        }
        txtRewardAmount.text = defaultQuestReward[0].amount.ToString();
    }
}
