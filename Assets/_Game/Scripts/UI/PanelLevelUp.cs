using AssetKits.ParticleImage.Enumerations;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelUp : UIPanel
{
    [SerializeField] Button closeBtn;
    [SerializeField] Button x2Btn;
    [SerializeField] RewardItemUI itemUIPrefab;
    [SerializeField] Transform rewardItemUIsContainer;
    List<RewardItemUI> rewardItemUIs;
    List<ItemData> rewards = new List<ItemData>();
    int spawnedCount;
    public Transform coinBar;

    [Header("Level up contents")]
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] Image levelSlideImg;
    [SerializeField] Transform LevelBar;
    [SerializeField] Transform LevelBarHold;
    [SerializeField] Transform LevelBarShow;
    [SerializeField] Transform popUp;
    Vector3 spinAngle = new Vector3(0, 0, 1 - 3 * 360);
    bool x2Taken;

    public override void Awake()
    {
        panelType = UIPanelType.PanelLevelUp;
        base.Awake();
        rewardItemUIs = new List<RewardItemUI>();
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(ClosePanel);
        x2Btn.onClick.AddListener(X2BtnOnClick);
    }

    private void OnEnable()
    {
        DisableOldItem();
        transform.SetAsLastSibling();
        coinBar.localScale = Vector3.zero;
        popUp.localScale = Vector3.zero;
        PlayAnim();
        InitRewardData();
        x2Taken = false;
    }

    void InitRewardData()
    {
        rewards.Clear();
        List<ItemData>  rewardsTemp = GameManager.Instance.rewardItems;
        for (int i = 0; i < rewardsTemp.Count; i++)
        {
            rewards.Add(rewardsTemp[i]);
        }
    }

    void PlayAnim()
    {
        closeBtn.gameObject.SetActive(false);
        x2Btn.gameObject.SetActive(false);
        levelTxt.text = (ProfileManager.Instance.playerData.playerResourseSave.currentLevel - 1).ToString();
        LevelBar.position = LevelBarHold.position;
        levelSlideImg.fillAmount = 0;
        LevelBar.localScale = Vector3.one;
        // State 1
        LevelBar.DOMove(LevelBarShow.position, 0.75f).SetDelay(0.25f);
        LevelBar.DORotate(spinAngle, 0.75f).SetDelay(0.25f);
        LevelBar.DOScale(1.5f, 0.5f).SetDelay(0.25f);

        // State 2
        LevelBar.DOScale(1.75f, 0.15f).SetEase(Ease.OutBack).SetDelay(1.5f);
        DOVirtual.Float(0, 1, 0.35f, (value) =>
        {
            levelSlideImg.fillAmount = value;
        }).SetDelay(1.5f);
        LevelBar.DOScale(2.35f, 0.2f).SetDelay(1.25f + 0.5f).OnComplete(() =>
        {
            levelTxt.text = (ProfileManager.Instance.playerData.playerResourseSave.currentLevel).ToString();
        });
        LevelBar.DOScale(2f, 0.05f).SetDelay(1.25f + 0.5f + 0.25f);

        LevelBar.DOScale(0, 0.25f).SetEase(Ease.InBack).SetDelay(3.25f);
        popUp.DOScale(1, 0.25f).SetEase(Ease.OutBack).SetDelay(3.5f).OnComplete(InitReward);
    }



    void InitReward()
    {
        spawnedCount = 0;
        StartCoroutine(SpawnReward());
    }

    IEnumerator SpawnReward()
    {
        yield return ConstantValue.WAIT_SEC01;
        if (spawnedCount < rewards.Count)
        {
            RewardItemUI rewardItemUI = GetRewardItemUI();
            rewardItemUI.Init(rewards[spawnedCount]);
            spawnedCount++;
            StartCoroutine(SpawnReward());
        }
        else
        {
            if (!x2Taken)
            {
                if (ProfileManager.Instance.playerData.playerResourseSave.currentLevel < 4)
                {
                    UIManager.instance.ShowPanelHint(rewards[1].ItemType);
                }
                GameManager.Instance.GetItemRewards(rewards);
                x2Btn.gameObject.SetActive(true);
            }
            yield return ConstantValue.WAIT_SEC1;
            yield return ConstantValue.WAIT_SEC1;
            closeBtn.gameObject.SetActive(true);
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
        newRewardItemUI.gameObject.SetActive(true);
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
            rewardItemUIs[i].gameObject.SetActive(false);
        }
        UIManager.instance.ClosePanelLevelUp();
        GameManager.Instance.cakeManager.cakeShowComponent.ShowNormalCake();
        GameManager.Instance.cakeManager.cakeShowComponent.ShowNextToUnlockCake();
        GameManager.Instance.cakeManager.SetOnMove(false);
        if (ProfileManager.Instance.playerData.playerResourseSave.currentLevel > 2)
            GameManager.Instance.ShowInter();
    }

    void X2BtnOnClick()
    {
        GameManager.Instance.ShowRewardVideo(WatchVideoRewardType.GetX2LevelUpReward, GetX2Reward);
    }

    void GetX2Reward()
    {
        closeBtn.gameObject.SetActive(false);
        x2Btn.gameObject.SetActive(false);
        x2Taken = true;
        GameManager.Instance.GetItemRewards(rewards);
        for (int i = 0; i < rewards.Count; i++)
        {
            rewards[i].amount *= 2;
        }
        DisableOldItem();
        InitReward();
    }
}
