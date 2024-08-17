using _BaseGame.ScriptableObjects.MapData;
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
    private void Start()
    {
        UpdateData();
    }

    public void UpdateData()
    {
        currentLevel = ProfileManager.Instance.playerData.playerResourseSave.currentLevel;

        mapCakeConfigsData = MapCakeConfigs.Instance.GetMapCakeConfigsData(currentLevel);

        orderProgress = ProfileManager.Instance.playerData.playerResourseSave.orderProgress;

        currentOrderIndex = orderProgress.currentOrderIndex;
    }

    public void OnAddProgress(int cakeID) { }
}
