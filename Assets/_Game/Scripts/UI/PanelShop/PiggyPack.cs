using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiggyPack : IAPPack
{
    [SerializeField] Slider slider;
    [SerializeField] float maxPiggy;
    public override void OnEnable()
    {
        base.OnEnable();
        ItemData coin = GetCoinReward();
        if(coin != null)
        {
            slider.value = coin.amount / maxPiggy;
        }
        else
        {
            slider.value = 0;
        }
    }

    ItemData GetCoinReward()
    {
        for (int i = 0; i < rewardItems.Count; i++)
        {
            if (rewardItems[i].ItemType == ItemType.Coin)
                return rewardItems[i];
        }
        return null;
    }
}
