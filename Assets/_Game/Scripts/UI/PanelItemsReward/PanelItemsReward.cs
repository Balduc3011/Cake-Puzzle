using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelItemsReward : UIPanel
{
    [SerializeField] RewardItemUI itemUIPrefab;
    [SerializeField] Transform rewardItemUIsContainer;
    List<RewardItemUI> rewardItemUIs;
    public override void Awake()
    {
        panelType = UIPanelType.PanelItemsReward;
        base.Awake();
    }

    public void Init()
    {

    }

    RewardItemUI GetRewardItemUI()
    {
        for (int i = 0; i < rewardItemUIs.Count; i++)
        {
            if (!rewardItemUIs[i].gameObject.activeSelf)
            {
                return rewardItemUIs[i];
            }
        }
        RewardItemUI newRewardItemUI = Instantiate(itemUIPrefab, rewardItemUIsContainer);
        return newRewardItemUI;
    }
}
