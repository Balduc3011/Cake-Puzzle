using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotCakeCheat : MonoBehaviour
{
    public Button btnChangeTier;
    public TextMeshProUGUI txtTier;
    public Image imgCake;
    public int cakeID;
    int currentTier = 0;
    private void Awake()
    {
        btnChangeTier.onClick.AddListener(ChangeTier);
        imgCake.sprite = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeIcon(cakeID, currentTier);
        txtTier.text = "Tier " + (currentTier+1);
    }

    public void ChangeTier() {
        currentTier = currentTier == 0 ? 1 : 0;
        imgCake.sprite = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeIcon(cakeID, currentTier);
        txtTier.text = "Tier " + (currentTier + 1);
    }
}
