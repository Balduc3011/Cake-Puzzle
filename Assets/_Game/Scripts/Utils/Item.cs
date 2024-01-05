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
    NoAds,
    Blood,
    Skeleton,
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public float amount;
}
