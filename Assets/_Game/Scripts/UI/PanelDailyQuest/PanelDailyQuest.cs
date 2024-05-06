using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelDailyQuest : UIPanel
{
    [SerializeField] Button closeBtn;
    [SerializeField] List<QuestTarget> targets = new List<QuestTarget>();
    [SerializeField] Slider processSlider;
    float process;
    float currentProcess;
    [SerializeField] float currentRiseSpeed;
    public override void Awake()
    {
        panelType = UIPanelType.PanelDailyQuest;
        base.Awake();
        closeBtn.onClick.AddListener(ClosePanel);
        EventManager.AddListener(EventName.ChangeStarDailyQuest.ToString(), Init);
    }
    private void OnEnable()
    {
        transform.SetAsLastSibling();
        process = -1;
        Init();
        currentRiseSpeed = ProfileManager.Instance.playerData.questDataSave.starsEarned > 0 ? ProfileManager.Instance.playerData.questDataSave.starsEarned : 1;
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

    public void Init()
    {
        currentProcess = ProfileManager.Instance.playerData.questDataSave.starsEarned;
    }

    private void Update()
    {
        if(process < currentProcess)
        {
            process += Time.deltaTime * currentRiseSpeed;
            processSlider.value = process;
            if(process >= currentProcess)
            {
                UpdateTarget();
                currentRiseSpeed = 10f;
            }
        }
    }

    void UpdateTarget()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].Init();
        }
    }
}
