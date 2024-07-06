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
    public Sprite GetItemSprite(string itemName)
    {
        for (int i = 0; i < itemSprites.Count; i++)
        {
            if (String.Compare(itemSprites[i].name, itemName) == 0)
            {
                return itemSprites[i];
            }
        }
        return null;
    }

    public Sprite GetCakeSprite(int cakeUnlockID)
    {
        CakeData cakeData = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(cakeUnlockID);
        if(cakeData != null)
        {
            int levelPref = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(cakeUnlockID);
            if (levelPref > 2) levelPref = 2;
            return cakeData.icons[levelPref - 1];
        }
        return null;
    }



}
