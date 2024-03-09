using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelDailyReward : UIPanel
{
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    public SheetAnimation sheetAnimation;
    Transform Transform;
    [SerializeField] Button closeBtn;
    [SerializeField] List<DailyItemUI> uiDailyItems;
    public override void Awake()
    {
        panelType = UIPanelType.PanelDailyReward;
        base.Awake();
        Transform = transform;
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        Init();
        Transform.SetAsLastSibling();
        sheetAnimation.PlayAnim();
    }

    void Init()
    {
        List<DailyRewardConfig> dailyRewardConfig = ProfileManager.Instance.dataConfig.dailyRewardDataConfig.dailyRewardConfig;
        for (int i = 0; i < uiDailyItems.Count; i++)
        {
            uiDailyItems[i].Init(i, dailyRewardConfig[i]);
        }
    }

    void ClosePanel()
    {
        //openAndCloseAnim.OnClose(CloseInstant);
        uiPanelShowUp.OnClose(CloseInstant);
    }

    void CloseInstant()
    {
        UIManager.instance.ClosePanelDailyReward();
    }
}
