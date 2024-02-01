using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyItemUI : MonoBehaviour
{
    DailyRewardConfig dailyRewardConfig;
    [SerializeField] int dayIndex;
    [SerializeField] Button itemBtn;
    [SerializeField] TextMeshProUGUI titleTxt;
    [SerializeField] List<ItemToShow> itemsToShow;
    [SerializeField] GameObject rewardedObj;
    [SerializeField] GameObject hideObj;
    [SerializeField] GameObject todayObj;
    [SerializeField] bool toHide;

    string STR_DAY = "DAY ";
    void Start()
    {
        itemBtn.onClick.AddListener(CollectItem);
    }

    void CollectItem()
    {
        GameManager.Instance.dailyRewardManager.OnGetDailyReward(dayIndex);
        SetInteract();
    }

    public void Init(int dayIndex, DailyRewardConfig data)
    {
        this.dayIndex = dayIndex;
        dailyRewardConfig = data;
        titleTxt.text = STR_DAY + (dailyRewardConfig.dayIndex + 1).ToString();
        dayIndex = dailyRewardConfig.dayIndex;
        for (int i = 0; i < itemsToShow.Count; i++)
        {
            itemsToShow[i].Init(dailyRewardConfig.rewardList[i]);
        }
        SetInteract();
    }

    void SetInteract()
    {
        itemBtn.interactable = GameManager.Instance.dailyRewardManager.IsAbleToGetDailyReward(dailyRewardConfig.dayIndex) &&
            !GameManager.Instance.dailyRewardManager.CheckDailyRewardCollectted(dailyRewardConfig.dayIndex);
        todayObj.SetActive(itemBtn.interactable);
        rewardedObj.SetActive(GameManager.Instance.dailyRewardManager.CheckDailyRewardCollectted(dailyRewardConfig.dayIndex));
        if (toHide)
        {
            hideObj.SetActive(!GameManager.Instance.dailyRewardManager.IsAbleToGetDailyReward(dailyRewardConfig.dayIndex) &&
                !GameManager.Instance.dailyRewardManager.CheckDailyRewardCollectted(dailyRewardConfig.dayIndex));
        }
    }
}

[System.Serializable]
public class ItemToShow
{
    [SerializeField] Image itemIconImg;
    [SerializeField] TextMeshProUGUI amoutTxt;

    public void Init(ItemData dailyRewardData)
    {
        amoutTxt.text = dailyRewardData.amount.ToString();
        itemIconImg.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(dailyRewardData.ItemType);
    }
}
