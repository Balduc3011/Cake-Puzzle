using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTarget : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] Button btn;
    [SerializeField] TextMeshProUGUI targetAmountTxt;
    [SerializeField] Image targetBar;
    [SerializeField] Color reachColor;
    [SerializeField] Color noneColor;
    [SerializeField] float require;
    [SerializeField] List<ItemData> rewards;
    [SerializeField] GameObject star;
    float delay;

    private void Start()
    {
        btn.onClick.AddListener(ClaimReward);
    }

    private void OnEnable()
    {
        //Init();
        transform.localScale = Vector3.zero;
        delay = 0f;
    }

    public void Init()
    {
        transform.DOScale(1, 0.25f).From(0).SetEase(Ease.OutBack).SetDelay(delay + index * 0.1f);
        targetAmountTxt.text = require.ToString();
        targetBar.color = (ProfileManager.Instance.playerData.questDataSave.starsEarned >= require ? reachColor : noneColor);
        btn.interactable = ProfileManager.Instance.playerData.questDataSave.starsEarned >= require;
        star.SetActive(ProfileManager.Instance.playerData.questDataSave.CheckCanEarnQuest(index, require));
        delay = 0;
    }

    public void ClaimReward()
    {
        if(ProfileManager.Instance.playerData.questDataSave.CheckCanEarnQuest(index, require))
        {
            InitReward();
            ProfileManager.Instance.playerData.questDataSave.GetReward(index);
            GameManager.Instance.GetItemRewards(rewards);
            UIManager.instance.ShowPanelItemsReward();
            Init();
        }
    }

    void InitReward()
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            if (rewards[i].ItemType == ItemType.Cake)
            {
                rewards[i].subId = ProfileManager.Instance.playerData.cakeSaveData.GetRandomOwnedCake();
                rewards[i].amount = (int)(Random.Range(5, 10));
            }
        }
    }
}
