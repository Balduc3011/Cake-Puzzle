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
    pack1 = 1,
    piggy_pack = 2,
    pack_hammer = 10,
    pack_fillup = 20,
    pack_reroll = 30,
    pack_money1 = 40,
    pack_money2 = 41,
    pack_money3 = 42,
}