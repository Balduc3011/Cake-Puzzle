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
    [SerializeField] bool toHide;

    string STR_DAY = "DAY ";
    void Start()
    {
        itemBtn.onClick.AddListener(CollectItem);
    }

    void CollectItem()
    {

    }

    public void Init(DailyRewardConfig data)
    {
        dailyRewardConfig = data;
        titleTxt.text = STR_DAY + dailyRewardConfig.dayIndex.ToString();
        dayIndex = dailyRewardConfig.dayIndex;
        for (int i = 0; i < itemsToShow.Count; i++)
        {
            itemsToShow[i].Init(i, dailyRewardConfig.rewardList[i]);
        }
        SetInteract();
    }

    void SetInteract()
    {
        //itemBtn.interactable = GameManager.Instance.dailyRewardManager.CheckAbleToCollect(dailyRewardConfig.dayIndex) &&
        //    !GameManager.Instance.CheckCollectted.CheckAbleToCollect(dailyRewardConfig.dayIndex);
        //rewardedObj.SetActive(GameManager.Instance.CheckCollectted.CheckAbleToCollect(dailyRewardConfig.dayIndex));    
        //if(toHide)
        //{
        //    hideObj.SetActive(!GameManager.Instance.dailyRewardManager.CheckAbleToCollect(dailyRewardConfig.dayIndex) &&
        //        !GameManager.Instance.CheckCollectted.CheckAbleToCollect(dailyRewardConfig.dayIndex));
        //}
    }
}

[System.Serializable]
public class ItemToShow
{
    [SerializeField] Image itemIconImg;
    [SerializeField] TextMeshProUGUI amoutTxt;

    public void Init(int dayIndex, DailyRewardData dailyRewardData)
    {
        amoutTxt.text = dailyRewardData.amount.ToString();
    }
}
