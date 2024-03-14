using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PiggyPack : IAPPack
{
    [SerializeField] Slider slider;
    [SerializeField] float maxPiggy;

    [SerializeField] TextMeshProUGUI valueTxt;
    [SerializeField] TextMeshProUGUI maxValueTxt;

    public override void OnEnable()
    {
        base.OnEnable();
        ItemData coin = GetCoinReward();
        if(coin != null)
        {
            coin.amount = ProfileManager.Instance.playerData.playerResourseSave.piggySave;
            slider.value = coin.amount / ConstantValue.VAL_MAX_PIGGY;
            valueTxt.text = coin.amount.ToString() + ConstantValue.STR_SLASH + ConstantValue.VAL_MAX_PIGGY.ToString();
            maxValueTxt.text = ConstantValue.VAL_MAX_PIGGY.ToString();
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
