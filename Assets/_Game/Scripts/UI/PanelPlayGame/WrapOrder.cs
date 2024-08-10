using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapOrder : MonoBehaviour
{
    [SerializeField] List<SlotOrder> slotOrders = new();


    private void Awake()
    {
        for (int i = 0; i < slotOrders.Count; i++)
        {
            int cakeID = ProfileManager.Instance.playerData.cakeSaveData.GetRandomOwnedCake();
            CakeData cakeData = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(cakeID);
            slotOrders[i].InitData(cakeData);
        }
     
    }
}
