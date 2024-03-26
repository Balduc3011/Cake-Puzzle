using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopDataConfig", menuName = "ScriptableObject/ShopDataConfig")]
public class ShopDataConfig : ScriptableObject
{
    public List<ShopPack> shopPacks = new List<ShopPack>();

    public ShopPack GetShopPack(OfferID packageId)
    {
        for (int i = 0; i < shopPacks.Count; i++)
        {
            if (shopPacks[i].packageId == packageId)
                return shopPacks[i];
        }
        return null;
    }

    public ShopPack GetShopPack(string packageName)
    {
        for (int i = 0; i < shopPacks.Count; i++)
        {
            if (shopPacks[i].packageId.ToString() == packageName)
                return shopPacks[i];
        }
        return null;
    }
}

[System.Serializable]
public class ShopPack
{
    public OfferID packageId;
    public List<ItemData> rewards = new();
}

public enum OfferID
{
    None = 0,
    Pack2 = 1,
    Pack1 = 2,
}