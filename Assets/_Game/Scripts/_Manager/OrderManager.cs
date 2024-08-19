using _BaseGame.ScriptableObjects.MapData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        return mapCakeConfigsData.GetCakeOrder(currentOrderIndex, cakeIndex);
    }

    public float GetTimeOrder()
    {
        return mapCakeConfigsData.orderCakeTime * 60;
    }

    public void DoneACake(int cakeID)
    {
        if (currentLevel == 0)
        {
            ProfileManager.Instance.playerData.playerResourseSave.LevelUp();
            return;
        }
        ProfileManager.Instance.playerData.cakeSaveData.AddCakeProgress(cakeID);
        UIManager.instance.panelGamePlay.wrapOrder.DoneACake(cakeID);
        
        orderProgress = ProfileManager.Instance.playerData.cakeSaveData.orderProgress;
        CheckDoneAll(false);
    }

    public void CheckDoneAll(bool checkFromFirstGame) {
        if (currentLevel == 0)
            return;
        if (IsDoneAll())
        {
            if(!checkFromFirstGame)
                ProfileManager.Instance.playerData.playerResourseSave.AddMoney(15);

            if (MapCakeConfigs.Instance.IsLastOrderOfLevel(currentLevel, currentOrderIndex + 1))
            {
                Debug.Log("Level up");
                ProfileManager.Instance.playerData.playerResourseSave.LevelUp();
            }
            else
                ProfileManager.Instance.playerData.cakeSaveData.NextOrder(mapCakeConfigsData);
            ProfileManager.Instance.playerData.cakeSaveData.SetTimeRemaining(DateTime.Now.AddMinutes(mapCakeConfigsData.orderCakeTime).ToString(new CultureInfo("en-US")));
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
