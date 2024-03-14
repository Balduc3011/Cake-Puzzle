using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PackType
{
    None,
    Pack1,
    Pack2,
    Pack3
}

public enum ItemType
{
    NoAds = -2,
    Cake = -1,
    None = 0,
    Trophy = 1,
    Coin = 2,
    Swap = 3,
    Hammer = 4,
    ReRoll = 5,
    Bomb = 6,
    FillUp = 7
}

[System.Serializable]
public class ItemData
{
    public ItemType ItemType;
    public int subId;
    public float amount;
}
