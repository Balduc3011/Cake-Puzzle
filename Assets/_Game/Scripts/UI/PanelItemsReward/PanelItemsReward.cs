using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelItemsReward : UIPanel
{
    [SerializeField] Transform titleTrs;
    [SerializeField] Button closeBtn;
    [SerializeField] RewardItemUI itemUIPrefab;
    [SerializeField] Transform rewardItemUIsContainer;
    List<RewardItemUI> rewardItemUIs;
    List<ItemData> rewards;
    int spawnedCount;

    Vector3 titleScale = new Vector3 (0f, 1f, 1f);
    public override void Awake()
    {
        panelType = UIPanelType.PanelItemsReward;
        base.Awake();
        titleTrs.localScale = titleScale;
        rewardItemUIs = new List<RewardItemUI>();
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        titleTrs.DOScaleX(1, 0.25f);
        DisableOldItem();
        InitReward();
    }

    void InitReward()
    {
        closeBtn.interactable = false;
        spawnedCount = 0;
        rewards = GameManager.Instance.rewardItems;
        //for (int i = 0; i < rewards.Count; i++)
        //{
        //    RewardItemUI rewardItemUI = GetRewardItemUI();
        //    rewardItemUI.Init(rewards[i]);
        //}
        StartCoroutine(SpawnReward());
    }

    IEnumerator SpawnReward()
    {
        yield return ConstantValue.WAIT_SEC025;
        if (spawnedCount < rewards.Count)
        {
            RewardItemUI rewardItemUI = GetRewardItemUI();
            rewardItemUI.Init(rewards[spawnedCount]);
            spawnedCount++;
            StartCoroutine(SpawnReward());
        }
        else
        {
            closeBtn.interactable = true;
        }
    }

    RewardItemUI GetRewardItemUI()
    {
        for (int i = 0; i < rewardItemUIs.Count; i++)
        {
            if (!rewardItemUIs[i].gameObject.activeSelf)
            {
                rewardItemUIs[i].gameObject.SetActive(true);
                return rewardItemUIs[i];
            }
        }
        RewardItemUI newRewardItemUI = Instantiate(itemUIPrefab, rewardItemUIsContainer);
        rewardItemUIs.Add(newRewardItemUI);
        return newRewardItemUI;
    }

    void DisableOldItem()
    {
        for (int i = 0; i < rewardItemUIs.Count; i++)
        {
            rewardItemUIs[i].gameObject.SetActive(false);
        }
    }

    void ClosePanel()
    {
        for (int i = 0; i < rewardItemUIs.Count; i++)
        {
            rewardItemUIs[i].gameObject.SetActive(false);
        }
        titleTrs.localScale = titleScale;
        UIManager.instance.ClosePanelItemsReward();
    }
}
