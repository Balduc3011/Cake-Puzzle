using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDataConfig", menuName = "ScriptableObject/SpriteDataConfig")]
public class SpriteDataConfig : ScriptableObject
{
    public List<Sprite> itemSprites;
    public List<Sprite> cakeSprite;

    public Sprite GetItemSprite(ItemType itemType)
    {
        for (int i = 0; i < itemSprites.Count; i++)
        {
            if (String.Compare(itemSprites[i].name, itemType.ToString()) == 0)
            {
                return itemSprites[i];
            }
        }
        return null;
    }

    public Sprite GetCakeSprite(int cakeUnlockID)
    {
        return cakeSprite[cakeUnlockID];
    }



}
