using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatPanel : UIPanel
{
    public Button btnExit;
    public Button btnSpawn;
    public Button btnCoin;
    public Button btnItem;
    public TMP_InputField totalPlateInput;

    public List<SlotCakeCheat> slots = new();

    int totalPlateCount;

    public override void Awake()
    {
        panelType = UIPanelType.PanelCheat;
        base.Awake();
        btnExit.onClick.AddListener(ExitPanel);
        btnSpawn.onClick.AddListener(Spawn);
        btnItem.onClick.AddListener(SetItem);
        btnCoin.onClick.AddListener(SetCoin);
    }

    void SetItem() {
        ProfileManager.Instance.playerData.playerResourseSave.SetItem(ItemType.Hammer);
        ProfileManager.Instance.playerData.playerResourseSave.SetItem(ItemType.FillUp);
        ProfileManager.Instance.playerData.playerResourseSave.SetItem(ItemType.ReRoll);
        EventManager.TriggerEvent(EventName.AddItem.ToString());
    }
    void SetCoin() {
        ProfileManager.Instance.playerData.playerResourseSave.SetCoin();
        EventManager.TriggerEvent(EventName.ChangeCoin.ToString());
    }

    void ExitPanel() {
        UIManager.instance.ClosePanelCheat();
    }

    void Spawn() {
        GameManager.Instance.cakeManager.ClearCake();
        totalPlateCount = int.Parse(totalPlateInput.text);
        totalPlateCount = totalPlateCount > 20 ? 20 : totalPlateCount;
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetData();
        }
        for (int i = 0; i < totalPlateCount; i++)
        {
            GameManager.Instance.cakeManager.LoadCakeCheat();
        }
    }
}
