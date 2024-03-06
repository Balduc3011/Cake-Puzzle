using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StreakEffect : ExpEffect
{
    public void SettingMaterial(int cakeID) {
        txtExpAdd.font = Resources.Load("Font/Font_" + cakeID) as TMP_FontAsset;
    }

    public override void ChangeText(string streak)
    {
        txtExpAdd.text = "STREAK +" + streak;
    }
}
