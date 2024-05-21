using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatPanel : MonoBehaviour
{
    public Button btnExit;
    public Button btnSpawn;
    public InputField totalPlateInput;

    public List<SlotCakeCheat> slots = new();

    int totalPlateCount;

    public void Awake()
    {
        btnExit.onClick.AddListener(ExitPanel);
        btnSpawn.onClick.AddListener(Spawn);
    }

    void ExitPanel() {
        gameObject.SetActive(false);
    }

    void Spawn() {
        totalPlateCount = int.Parse(totalPlateInput.text);
    }
}
