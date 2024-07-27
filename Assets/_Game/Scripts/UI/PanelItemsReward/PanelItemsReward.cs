using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelItemsReward : UIPanel
{
    [SerializeField] Transform titleTrs;
    [SerializeField] Button closeBtn;
    [SerializeField] GameObject closeTxt;
    [SerializeField] RewardItemUI itemUIPrefab;
    [SerializeField] Transform rewardItemUIsContainer;
    List<RewardItemUI> rewardItemUIs;
    List<ItemData> rewards;
    int spawnedCount;
    public Transform popup;
    public Transform coinBar;
    public Transform bagBar;

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
        for (int i = 0; i < rewardItemUIs.Count; i++)
        {
            rewardItemUIs[i].gameObject.SetActive(false);
        }
        titleTrs.DOScaleX(1, 0.25f);
        DisableOldItem();
        InitReward();
        transform.SetAsLastSibling();
        coinBar.DOScale(1, 0.25f).From(0).SetDelay(1f);
        bagBar.DOScale(1, 0.25f).From(0).SetDelay(1f);
        popup.DOScale(1, 0.25f).From(0);
    }

    void InitReward()
    {
        closeBtn.interactable = false;
        closeTxt.SetActive(false);
        spawnedCount = 0;
        rewards = GameManager.Instance.rewardItems;
        StartCoroutine(SpawnReward());
    }

    IEnumerator SpawnReward()
    {
        yield return ConstantValue.WAIT_SEC01;
        if (spawnedCount < rewards.Count)
        {
            RewardItemUI rewardItemUI = GetRewardItemUI();
            rewardItemUI.Init(rewards[spawnedCount], this);
            spawnedCount++;
            StartCoroutine(SpawnReward());
        }
        else
        {
            closeBtn.interactable = true;
            closeTxt.SetActive(true);
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
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        for (int i = 0; i < rewardItemUIs.Count; i++)
        {
            rewardItemUIs[i].PlayEffect();
        }
        titleTrs.localScale = titleScale;
        DOVirtual.DelayedCall(1.5f, CloseInstant);
    }

    void CloseInstant()
    {
        UIManager.instance.ClosePanelItemsReward();
    }
}
