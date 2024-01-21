using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSetting : UIPanel
{
    Transform Transform;
    [SerializeField] Button closeBtn;
    [SerializeField] Button toMenuBtn;
    public override void Awake()
    {
        panelType = UIPanelType.PanelSetting;
        base.Awake();
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClose);
        toMenuBtn.onClick.AddListener(BackToMenu);
        Transform = transform;
    }

    private void OnEnable()
    {
        toMenuBtn.gameObject.SetActive(GameManager.Instance.playing);
        if(Transform == null) Transform = transform;
        Transform.SetAsLastSibling();
    }

    void OnClose()
    {
        openAndCloseAnim.OnClose(UIManager.instance.ClosePanelSetting);
    }

    void BackToMenu()
    {
        GameManager.Instance.BackToMenu();
        OnClose();
    }
}
