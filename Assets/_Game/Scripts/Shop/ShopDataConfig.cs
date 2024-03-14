using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopDataConfig", menuName = "ScriptableObject/ShopDataConfig")]
public class ShopDataConfig : ScriptableObject
{
    public List<ShopPack> shopPacks = new List<ShopPack>();

    public ShopPack GetShopPack(PackageId packageId)
    {
        for (int i = 0; i < shopPacks.Count; i++)
        {
            if (shopPacks[i].packageId == packageId)
                return shopPacks[i];
        }
        return null;
    }
}

[System.Serializable]
public class ShopPack
{
    public PackageId packageId;
    public List<ItemData> rewards = new();
}

public enum PackageId
{
    None = 0,
    Piggy = 1,
    Pack1 = 2,
}