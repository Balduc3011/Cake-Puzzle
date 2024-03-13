using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public List<ItemData> rewardItems;
    private void Start()
    {
        cameraManager.ShowARoom(0);
        AddTempName();
    }
    public void PlayGame()
    {
        UIManager.instance.ShowPanelPlayGame();
        cakeManager.PlayGame();
        playing = true;
    }

    public void BackToMenu()
    {
        UIManager.instance.ClosePanelPlayGame();
        UIManager.instance.panelTotal.BackToMenu();
        cameraManager.ShowARoom(0);
        cameraManager.CloseMainCamera();
        playing = false;
    }

    public float GetDefaultCakeProfit()
    {
        return ConstantValue.VAL_DEFAULT_EXP * (ProfileManager.Instance.playerData.playerResourseSave.HasX2Booster() ? 2 : 1);
    }

    public void GetLevelUpReward()
    {
        rewardItems.Clear();
        GetCakeOnLevelUp();
        GetLevelUpItem();
        CollectItemReward(rewardItems);
    }

    void GetCakeOnLevelUp()
    {
        rewardItems.Clear();
        ItemData firstCake = new();
        firstCake.ItemType = ItemType.Cake;
        int newCakeID = ProfileManager.Instance.dataConfig.levelDataConfig.GetCakeID(ProfileManager.Instance.playerData.playerResourseSave.currentLevel - 1);
        if (newCakeID != -1)
        {
            ProfileManager.Instance.playerData.cakeSaveData.AddCake(newCakeID);
            ProfileManager.Instance.playerData.cakeSaveData.UseCake(newCakeID);
            firstCake.amount = 1;
        }
        else
        {
            newCakeID = ProfileManager.Instance.dataConfig.cakeDataConfig.GetRandomCake();
            firstCake.amount = UnityEngine.Random.Range(1, 6);
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
        extraCake.amount = UnityEngine.Random.Range(1, 6);
        rewardItems.Add(firstCake);
        rewardItems.Add(extraCake);
    }

    void GetLevelUpItem()
    {
        ItemData newItem = new();
        newItem.ItemType = ProfileManager.Instance.dataConfig.itemDataConfig.GetRewardItemOnLevel();
        newItem.amount = UnityEngine.Random.Range(1, 5);
        rewardItems.Add(newItem);
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

    public void AddPiggySave()
    {
        ProfileManager.Instance.playerData.playerResourseSave.AddPiggySave();
    }

    public bool IsHasNoAds()
    {
        return ProfileManager.Instance.playerData.playerResourseSave.IsHaveItem(ItemType.NoAds);
    }
}

