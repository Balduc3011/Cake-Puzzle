using _BaseGame.ScriptableObjects.MapData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrderManager : Singleton<OrderManager>
{
    public MapCakeConfigsData mapCakeConfigsData;
    CakeOrder cakeOrderTemp;
    public int currentLevel = 0;
    public int currentOrderIndex = 0;
    public OrderProgress orderProgress;
    public bool isFail;
    float timeRemaining;
    bool startOrder;
    private void Start()
    {
        EventManager.AddListener(EventName.ChangeLevel.ToString(), UpdateData);
    }

    private void Update()
    {
        if (startOrder)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                UIManager.instance.panelGamePlay.wrapOrder.ChangeTextTime(timeRemaining);
            }
            else {
                isFail = true;
                startOrder = false;
                UIManager.instance.ShowPanelLevelComplete(false);
                //gameObject.SetActive(false);
            }
        }
    }
    Tween tweenLoopSaveData;

    public void SetStartOrder(bool active) {
        if (active && timeRemaining > 0)
        {
            startOrder = active;
            return;
        }
        startOrder = active;
    }

    public void StartOrder() {
        startOrder = true;
        Debug.Log("start order");
        tweenLoopSaveData = DOVirtual.DelayedCall(3f, () => {
            if (!startOrder)
            {
                if (tweenLoopSaveData != null)
                    tweenLoopSaveData.Kill();
            }
            else ProfileManager.Instance.playerData.cakeSaveData.SetTimeRemaining(timeRemaining); 
        });
        tweenLoopSaveData.SetLoops(-1);
        tweenLoopSaveData.Play();
    }

    public void UpdateData()
    {
        //Debug.Log("Update data order");
        currentLevel = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;

        if (currentLevel == 0) return;

        mapCakeConfigsData = MapCakeConfigs.Instance.GetMapCakeConfigsData(currentLevel);

        orderProgress = ProfileManager.Instance.playerData.cakeSaveData.orderProgress;

        if (orderProgress == null || IsDifferentLevel())
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

    bool IsDifferentLevel() {
        int levelTemp = currentLevel;
        if (currentLevel > MapCakeConfigs.Instance.mapCakeConfigs.Count)
            levelTemp = currentLevel % MapCakeConfigs.Instance.mapCakeConfigs.Count;
        return levelTemp != orderProgress.currentOrderLevel;
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
            startOrder = false;
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
            //Debug.Log("current level: " + currentLevel);
            if (MapCakeConfigs.Instance.IsLastOrderOfLevel(currentLevel, currentOrderIndex + 1))
            {
                //Debug.Log("Level up");
                startOrder = false;
                ProfileManager.Instance.playerData.playerResourseSave.LevelUp();
            }
            else
            {
                //Debug.Log("next order");
                ProfileManager.Instance.playerData.cakeSaveData.NextOrder(mapCakeConfigsData);
            }
            float totalOrder = mapCakeConfigsData.cakeOrders.Count / 3;
            ProfileManager.Instance.playerData.cakeSaveData.SetTimeRemaining((mapCakeConfigsData.orderCakeTime * 60) / totalOrder);
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
        
        float totalOrder = mapCakeConfigsData.cakeOrders.Count / 3;
        ProfileManager.Instance.playerData.cakeSaveData.SetTimeRemaining((mapCakeConfigsData.orderCakeTime * 60) / totalOrder);
        timeRemaining = ProfileManager.Instance.playerData.cakeSaveData.GetTimeRemaining();

        isFail = false;
       
        UIManager.instance.panelGamePlay.wrapOrder.gameObject.SetActive(true);
        UIManager.instance.panelGamePlay.wrapOrder.GetOrder();
    }
}
