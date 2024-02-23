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
    None = 0,
    Trophy = 1,
    Coin = 2,
    Swap = 3,
    Hammer = 4,
    ReRoll = 5,
    Bomb = 6,
    FillUp
}

[System.Serializable]
public class ItemData
{
    public ItemType ItemType;
    public float amount;
}
