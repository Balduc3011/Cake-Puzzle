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
    public int currentLevel = 0;
    public int currentOrderIndex = 0;
    public OrderProgress orderProgress;
    public bool isFail;
    float timeRemaining;

    private void Start()
    {
        EventManager.AddListener(EventName.ChangeLevel.ToString(), UpdateData);
    }

    public void UpdateData()
    {
        Debug.Log("Update data order");
        currentLevel = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;

        if (currentLevel == 0) return;

        mapCakeConfigsData = MapCakeConfigs.Instance.GetMapCakeConfigsData(currentLevel);

        orderProgress = ProfileManager.Instance.playerData.cakeSaveData.orderProgress;

        if (orderProgress == null || orderProgress.currentOrderLevel != currentLevel)
        {
            orderProgress = ProfileManager.Instance.playerData.cakeSaveData.InitprogressData(currentLevel, mapCakeConfigsData);
        }

        currentOrderIndex = orderProgress.currentOrderIndex;

        timeRemaining = ProfileManager.Instance.playerData.cakeSaveData.GetTimeRemaining();
        if (timeRemaining == 0)
        {
            GameManager.Instance.cakeManager.isLooseByOrder = true;
            ProfileManager.Instance.playerData.cakeSaveData.ResetProgres();
            ProfileManager.Instance.playerData.cakeSaveData.SetTimeRemainingOrder();
            GameManager.Instance.cakeManager.InitCakeFromLevelData();
        }

        UIManager.instance.panelGamePlay?.wrapOrder.GetOrder();
        GameManager.Instance.cakeManager.isLooseByOrder = false;
    }

    public CakeOrder GetCakeOrder(int cakeIndex)
    {
        if (mapCakeConfigsData == null)
            UpdateData();
        return mapCakeConfigsData.GetCakeOrder(currentOrderIndex, cakeIndex);
    }

    public float GetTimeOrder()
    {
        if (timeRemaining == 0)
        {
            float totalOrder = mapCakeConfigsData.cakeOrders.Count / 3;
            return (mapCakeConfigsData.orderCakeTime * 60) / totalOrder;
        }
        else return timeRemaining;
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
