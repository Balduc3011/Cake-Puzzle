using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelDailyQuest : UIPanel
{
    [SerializeField] Button closeBtn;
    public override void Awake()
    {
        panelType = UIPanelType.PanelDailyQuest;
        base.Awake();
        closeBtn.onClick.AddListener(ClosePanel);
    }
    void ClosePanel()
    {
        ProfileManager.Instance.playerData.playerResourseSave.SaveRecord();
        openAndCloseAnim.OnClose(CloseInstant);
    }

    void CloseInstant()
    {
        UIManager.instance.ClosePanelDailyQuest();
    }
}
