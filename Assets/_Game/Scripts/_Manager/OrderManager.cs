using _BaseGame.ScriptableObjects.MapData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : Singleton<OrderManager>
{
    public MapCakeConfigsData mapCakeConfigsData;
    CakeOrder cakeOrderTemp;
    int currentLevel = 0;
    int currentOrderIndex = 0;
    public OrderProgress orderProgress;
    public bool isFail;

    private void Start()
    {
        EventManager.AddListener(EventName.ChangeLevel.ToString(), UpdateData);
    }

    public void UpdateData()
    {
        currentLevel = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;

        if (currentLevel == 0) return;

        mapCakeConfigsData = MapCakeConfigs.Instance.GetMapCakeConfigsData(currentLevel);

        orderProgress = ProfileManager.Instance.playerData.cakeSaveData.orderProgress;

        if (orderProgress == null || orderProgress.currentOrderLevel != currentLevel)
        {
            orderProgress = ProfileManager.Instance.playerData.cakeSaveData.InitprogressData(currentLevel, mapCakeConfigsData);
        }

        currentOrderIndex = orderProgress.currentOrderIndex;

        UIManager.instance.panelGamePlay?.wrapOrder.GetOrder();
    }

    public CakeOrder GetCakeOrder(int cakeIndex)
    {
        if (mapCakeConfigsData == null)
            UpdateData();
        Debug.Log($"current order index {currentOrderIndex * 3 + cakeIndex}");
        return mapCakeConfigsData.GetCakeOrder(currentOrderIndex, cakeIndex);
    }

    public void DoneACake(int cakeID)
    {
        ProfileManager.Instance.playerData.cakeSaveData.AddCakeProgress(cakeID);
        UIManager.instance.panelGamePlay.wrapOrder.DoneACake(cakeID);
        
        orderProgress = ProfileManager.Instance.playerData.cakeSaveData.orderProgress;

        if (IsDoneAll())
        {
            ProfileManager.Instance.playerData.playerResourseSave.AddMoney(15);
            if (MapCakeConfigs.Instance.IsLastOrderOfLevel(currentLevel, currentOrderIndex+1))
            {
                Debug.Log("Level up");
                ProfileManager.Instance.playerData.playerResourseSave.LevelUp();
            }
            else
            {
                ProfileManager.Instance.playerData.cakeSaveData.NextOrder(mapCakeConfigsData);
            }

            UIManager.instance.panelGamePlay.wrapOrder.OnOrderComplete(true, UpdateData);


        }
    }

    public bool IsDoneAll()
    {
        return orderProgress.IsDoneAll();
    }

    public bool ShowOrder()
    {
        return true;
    }

    public void Revive()
    {
        isFail = false;
        UIManager.instance.panelGamePlay.wrapOrder.gameObject.SetActive(true);
        UIManager.instance.panelGamePlay.wrapOrder.GetOrder();
    }
}
