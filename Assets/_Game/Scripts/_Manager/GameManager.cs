using ABI;
using DG.Tweening.Core.Easing;
using Firebase.RemoteConfig;
using SDK;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    public bool playing = false;
    public CameraManager cameraManager;
    public AudioManager audioManager;
    public CakeManager cakeManager;
    public ObjectPooling objectPooling;
    public SpinManager spinManager;
    public DailyRewardManager dailyRewardManager;
    public DecorationManager decorationManager;
    public ItemManager itemManager;
    public LightManager lightManager;
    public QuestManager questManager;
    public QuickTimeEventManager quickTimeEventManager;
    [field: SerializeField] public TutorialManager tutorialManager { get; private set; }
    public List<ItemData> rewardItems;
    [SerializeField] float showAdsCounter;
    [SerializeField] float showAdsCooldown = 30;
    [SerializeField] bool interShowing;
    private void Start()
    {
        cameraManager.ShowARoom(0);
        AddTempName();
        ShowCollapsibleBanner();
        showAdsCooldown = (float)(AdsManager.Instance.inter_show_cooldown);
        if(showAdsCooldown == 0) showAdsCooldown = 60;
    }
    public void PlayGame()
    {
        UIManager.instance.ShowPanelPlayGame();
        cakeManager.PlayGame();
        playing = true;
        if(tutorialManager.TutorialCondition())
        {
            tutorialManager.PlayTutorial();
        }
    }

    public void BackToMenu()
    {
        UIManager.instance.ClosePanelPlayGame();
        UIManager.instance.panelTotal.BackToMenu();
        cameraManager.ShowARoom(0);
        cameraManager.CloseMainCamera();
        playing = false;
        ShowCollapsibleBanner();
    }

    public float GetDefaultCakeProfit(int cakeID, int level, bool booster = false)
    {
        return (ConstantValue.VAL_DEFAULT_EXP + ConstantValue.VAL_DEFAULT_CAKE_ID * cakeID + ConstantValue.VAL_DEFAULT_CAKE_LEVEL * (level - 1))
            * (booster ? (ProfileManager.Instance.playerData.playerResourseSave.HasX2Booster() ? 2 : 1) : 1);
    }

    public void GetLevelUpReward(bool getLevelCake = true)
    {
        rewardItems.Clear();
        InitLevelUpRewards();
        if (getLevelCake) 
            GetLevelCake();
    }

    void GetLevelCake()
    {
        int level = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;
        ItemData firstCake = new();
        firstCake.ItemType = ItemType.Cake;
        int newCakeID = ProfileManager.Instance.dataConfig.levelDataConfig.GetCakeID(level - 1);
        firstCake.amount = 1;
        if (newCakeID != -1)
        {
            ProfileManager.Instance.playerData.cakeSaveData.AddCake(newCakeID);
            ProfileManager.Instance.playerData.cakeSaveData.UseCake(newCakeID);
        }
        firstCake.subId = newCakeID;
        AddItem(firstCake);
    }

    void AddRewardByType(ItemType type)
    {
        ItemData newItem = new();
        newItem.ItemType = type;
        newItem.subId = -1;
        switch (type)
        {
            case ItemType.Cake:
                newItem.amount = 5;
                ColectRewardCakeCard((int)newItem.amount);
                break;
            case ItemType.Swap:
            case ItemType.Hammer:
            case ItemType.ReRoll:
            case ItemType.Bomb:
            case ItemType.FillUp:
                newItem.amount = 1;
                break;
            case ItemType.Coin:
                newItem.amount = (int)(UnityEngine.Random.Range(10, 15));
                break;
            default:
                break;
        }
        rewardItems.Add(newItem);
    }

    void ColectRewardCakeCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randonCake = ProfileManager.Instance.playerData.cakeSaveData.GetRandomUnlockedCake();
            ProfileManager.Instance.playerData.cakeSaveData.AddCakeCard(randonCake, 1);
        }
    }

    [Button]
    public void CallTest(int testTime = 500)
    {
        StartCoroutine(TestRandonReward(testTime));
    }

    IEnumerator TestRandonReward(int testTime)
    {
        for (int i = 0; i < testTime; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(0.005f);
            RandonReward();
        }
    }

    public void RandonReward()
    {
        RandonRewardOneCake();
    }

    public void RandonRewardOneCake()
    {
        rewardItems.Clear();
        ProfileManager.Instance.dataConfig.itemDataConfig.InitRewardRandonList();

        ItemData newItem1 = new();
        newItem1.ItemType = ItemType.Cake;
        newItem1.subId = ProfileManager.Instance.playerData.cakeSaveData.GetRandomUnlockedCake();
        newItem1.amount = UnityEngine.Random.Range(1, 6);
        ProfileManager.Instance.dataConfig.itemDataConfig.RemoveFromTemp(ItemType.Cake);

        ItemData newItem2 = new();
        newItem2.ItemType = ProfileManager.Instance.dataConfig.itemDataConfig.GetRewardItemOnLevel();
        newItem2.subId = -1;
        if (newItem2.ItemType == ItemType.Coin)
        {
            newItem2.amount = (int)(UnityEngine.Random.Range(10, 15));
        }
        else
        {
            newItem2.amount = 1;
        }
        
        if(UnityEngine.Random.Range(-1f, 1f) > 0)
        {
            rewardItems.Add(newItem1);
            rewardItems.Add(newItem2);
        }
        else
        {
            rewardItems.Add(newItem2);
            rewardItems.Add(newItem1);
        }
    }

    public void InitLevelUpRewards()
    {
        rewardItems.Clear();

        ItemData newItem1 = new();
        newItem1.ItemType = ItemType.Cake;
        newItem1.subId = ProfileManager.Instance.playerData.cakeSaveData.GetRandomUnlockedCake();
        newItem1.amount = UnityEngine.Random.Range(1, 6);
        rewardItems.Add(newItem1);
        GetRewardByLevel();
    }

    void GetRewardByLevel()
    {
        int level = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;
        if (level == 1)
        {
            AddRewardByType(ItemType.Hammer);
            AddRewardByType(ItemType.Coin);
        }
        else if (level == 2)
        {
            AddRewardByType(ItemType.FillUp);
            AddRewardByType(ItemType.Coin);
        }
        else if (level == 3)
        {
            AddRewardByType(ItemType.ReRoll);
            AddRewardByType(ItemType.Coin);
        }
        else 
        {
            ProfileManager.Instance.dataConfig.itemDataConfig.InitRewardRandonList();
            ItemData newItem1 = new();
            newItem1.ItemType = ItemType.Cake;
            newItem1.subId = ProfileManager.Instance.playerData.cakeSaveData.GetRandomUnlockedCake();
            int cakeTry = 0;
            while (newItem1.subId == rewardItems[0].subId && cakeTry < 10)
            {
                newItem1.subId = ProfileManager.Instance.playerData.cakeSaveData.GetRandomUnlockedCake();
                cakeTry++;
            }
            newItem1.amount = UnityEngine.Random.Range(1, 6);
            ProfileManager.Instance.dataConfig.itemDataConfig.RemoveFromTemp(ItemType.Cake);
            ItemData newItem2 = new();
            newItem2.ItemType = ProfileManager.Instance.dataConfig.itemDataConfig.GetRewardItemOnLevel();
            newItem2.subId = -1;
            if (newItem2.ItemType == ItemType.Coin)
            {
                newItem2.amount = (int)(UnityEngine.Random.Range(10, 15));
            }
            else
            {
                newItem2.amount = 1;
            }
            rewardItems.Add(newItem1);
            rewardItems.Add(newItem2);
        }
    }

    bool CheckHasReward(ItemType itemType)
    {
        if(rewardItems == null || rewardItems.Count == 0)
            return false;
        for (int i = 0; i < rewardItems.Count; i++)
        {
            if (rewardItems[i].ItemType == itemType)
                return true;
        }
        return false;
    }

    void GetCakeOnLevelUp()
    {
        rewardItems.Clear();
        ItemData firstCake = new();
        firstCake.ItemType = ItemType.Cake;
        int newCakeID = ProfileManager.Instance.dataConfig.levelDataConfig.GetCakeID(ProfileManager.Instance.playerData.playerResourseSave.currentLevel - 1);
        firstCake.amount = UnityEngine.Random.Range(5, 10);
        if (newCakeID != -1)
        {
            ProfileManager.Instance.playerData.cakeSaveData.AddCake(newCakeID);
            ProfileManager.Instance.playerData.cakeSaveData.UseCake(newCakeID);
        }
        else
        {
            newCakeID = ProfileManager.Instance.dataConfig.cakeDataConfig.GetRandomCake();
        }
        firstCake.subId = newCakeID;
        int extraCakeId = ProfileManager.Instance.playerData.cakeSaveData.GetRandomUnlockedCake();
        while (extraCakeId == firstCake.subId)
        {
            extraCakeId = ProfileManager.Instance.playerData.cakeSaveData.GetRandomUnlockedCake();
        }
        ItemData extraCake = new();
        extraCake.ItemType = ItemType.Cake;
        extraCake.subId = extraCakeId;
        extraCake.amount = UnityEngine.Random.Range(5, 10);
        rewardItems.Add(firstCake);
        rewardItems.Add(extraCake);
    }

    public void GetItemRewards(List<ItemData> items)
    {
        rewardItems.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            rewardItems.Add(items[i]);
            //AddItem(items[i]);
        }
        CollectItemReward(items);
    }

    void CollectItemReward(List<ItemData> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            AddItem(items[i]);
        }
        EventManager.TriggerEvent(EventName.AddItem.ToString());
    }

    public void AddItem(ItemData item)
    {
        if (item.ItemType != ItemType.Cake)
            ProfileManager.Instance.playerData.playerResourseSave.AddItem(item);
        else
            ProfileManager.Instance.playerData.cakeSaveData.AddCakeCard(item.subId, (int)item.amount);
    }

    public float GetItemAmount(ItemType itemType)
    {
        return ProfileManager.Instance.playerData.playerResourseSave.GetItemAmount(itemType);
    }

    public void ClearAllCake()
    {
        cakeManager.ClearAllCake();
    }

    List<string> tempName;
    void AddTempName()
    {
        tempName = new List<string>();
        tempName.Add("Radago");
        tempName.Add("GoonFray");
        tempName.Add("Marikan");
        tempName.Add("GoonGwen");
        tempName.Add("Malenyn");
        tempName.Add("Miqueler");
        tempName.Add("Magget");
        tempName.Add("Goondrake");
        tempName.Add("Moogo");
        tempName.Add("Randan");
        tempName.Add("Raneen");
        tempName.Add("Richard");
        tempName.Add("Alexander");
        tempName.Add("Patcher");
    }

    public string GetRandomName()
    {
        return tempName[UnityEngine.Random.Range(0, tempName.Count)];
    }

    public void AddPiggySave(float amount)
    {
        ProfileManager.Instance.playerData.playerResourseSave.AddPiggySave(amount);
    }

    public bool IsHasNoAds()
    {
        if(ProfileManager.Instance.versionStatus == VersionStatus.Cheat) return true;
        return ProfileManager.Instance.playerData.playerResourseSave.IsHaveItem(ItemType.NoAds);
    }

    public void OnBuyPackSuccess(OfferID packageId)
    {
        switch (packageId)
        {
            case OfferID.None:
                break;
            case OfferID.pack1:
                rewardItems.Clear();
                ProfileManager.Instance.playerData.playerResourseSave.ClearPiggySave();
                ItemData itemData = new ItemData();
                itemData.ItemType = ItemType.Coin;
                itemData.amount = ProfileManager.Instance.playerData.playerResourseSave.piggySave;
                rewardItems.Add(itemData);
                CollectItemReward(rewardItems);
                break;
            case OfferID.piggy_pack:
                ShopPack shopPack = ProfileManager.Instance.dataConfig.shopDataConfig.GetShopPack(OfferID.piggy_pack);
                if(shopPack != null)
                {
                    GetItemRewards(shopPack.rewards);
                }
                break;
            default:
                break;
        }
    }

    public void ShowRewardVideo(WatchVideoRewardType watchVideoRewardType, UnityAction callBack)
    {
        if (IsHasNoAds())
        {
            questManager.AddProgress(QuestType.WatchADS, 1);
            if (callBack != null)
                callBack();
        } 
        else
        {
#if UNITY_EDITOR
            if(callBack != null)
            {
                callBack();
                questManager.AddProgress(QuestType.WatchADS, 1);
                return;
            }
#endif
            questManager.AddProgress(QuestType.WatchADS, 1);
            AdsManager.Instance.ShowRewardVideo(watchVideoRewardType.ToString(), callBack);
        }  
    }

    public void ShowCollapsibleBanner()
    {
        AdsManager.Instance.ShowBannerAds();
    }

    public void ShowInterRest()
    {
        if (IsHasNoAds()) return;
        if (!AdsManager.Instance.IsInterstitialAdLoaded()) return;
        if (ProfileManager.Instance.versionStatus == VersionStatus.Cheat) return;
        if (ProfileManager.Instance.playerData.playerResourseSave.currentLevel >= 1)
        {
            UIManager.instance.ShowPanelPreAds();
            showAdsCounter = 0;
            interShowing = true;
        }
            
    }

    public void ShowInter()
    {
        if (IsHasNoAds()) return;
        if (!AdsManager.Instance.IsInterstitialAdLoaded()) return;
        if (ProfileManager.Instance.versionStatus == VersionStatus.Cheat) return;
        AdsManager.Instance.ShowInterstitial();
        showAdsCounter = 0;
    }

    public void ResetCounter()
    {
        showAdsCounter = 0;
        interShowing = false;
    }

    

    private void Update()
    {
        if(!interShowing && playing)
        {
            showAdsCounter += Time.deltaTime;
            if (showAdsCounter > showAdsCooldown)
            {
                ShowInterRest();
            }
        }
    }
}

public enum WatchVideoRewardType
{
    NONE,
    X2CLAIM,
    UNLOCKITEM,
    TEST_ADS,
    GetFreeBooster,
    FreeCoinAds,
    X2RewardAds,
    CollectCoinAds,
    BoosterPlayTime,
    TimeBooster,
    UpgradeCake,
    GameOverRevive,
    FreeSpinAds,
    GetExtraCard,
    GetX2LevelUpReward
}