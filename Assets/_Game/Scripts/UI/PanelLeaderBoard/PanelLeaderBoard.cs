using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLeaderBoard : UIPanel
{
    [SerializeField] Button confirmBtn;
    [SerializeField] Button bgBtn;
    [SerializeField] List<UITopup> uITopups;
    [SerializeField] Transform topPos;
    [SerializeField] Transform botPos;
    public override void Awake()
    {
        panelType = UIPanelType.PanelLeaderBoard;
        base.Awake();
    }

    private void OnEnable()
    {
        DOVirtual.DelayedCall(0.5f, OnPlay);
        transform.SetAsLastSibling();
    }
    void Start()
    {
        confirmBtn.onClick.AddListener(ClosePanel);
        bgBtn.onClick.AddListener(ClosePanel);
        Setup();
    }

    public void OnPlay()
    {
        for (int i = 0; i < uITopups.Count; i++)
        {
            uITopups[i].OnPlay();
        }
    }

    void ClosePanel()
    {
        ProfileManager.Instance.playerData.playerResourseSave.SaveRecord();
        openAndCloseAnim.OnClose(CloseInstant);
    }

    void CloseInstant()
    {
        UIManager.instance.ClosePanelLeaderBoard();
    }

    void Setup()
    {
        for (int i = 0; i < uITopups.Count; i++)
        {
            uITopups[i].Setup(i, topPos, botPos);
        }
    }
}
