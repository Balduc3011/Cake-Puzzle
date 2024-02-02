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
    public CakeManager cakeManager;
    public ObjectPooling objectPooling;
    public SpinManager spinManager;
    public DailyRewardManager dailyRewardManager;
    public DecorationManager decorationManager;
    public ItemManager itemManager;
    public LightManager lightManager;
    public List<ItemData> rewardItems;

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
        playing = false;
    }

    public void GetItemReward(List<ItemData> items)
    {
        rewardItems.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            rewardItems.Add(items[i]);
            AddItem(items[i]);
        }
    }

    public void AddItem(ItemData item)
    {
        ProfileManager.Instance.playerData.playerResourseSave.AddItem(item);
    }

    public float GetItemAmount(ItemType itemType)
    {
        return ProfileManager.Instance.playerData.playerResourseSave.GetItemAmount(itemType);
    }

    public void ClearAllCake()
    {
        cakeManager.ClearAllCake();
    }
}

