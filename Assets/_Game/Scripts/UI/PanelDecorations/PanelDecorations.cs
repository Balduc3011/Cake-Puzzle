using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelDecorations : UIPanel
{
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    [SerializeField] DecorationType currentDecoration;
    [SerializeField] DecorSlotUI decorSlotUIPrefab;
    [SerializeField] Transform decorSlotContainer;
    List<DecorSlotUI> decorSlotUIs = new List<DecorSlotUI>();
    [SerializeField] List<DecorNavButton> decorNavButtonList;
    DecorNavButton selectedNavBtn;
    [SerializeField] RectTransform RawImageRect;
    [SerializeField] RectTransform showingSize;
    public override void Awake()
    {
        panelType = UIPanelType.PanelDecorations;
        base.Awake();
        RawImageRect.sizeDelta = new Vector2(showingSize.rect.width, showingSize.rect.width);
    }

    private void OnEnable()
    {
        InitDecorationData(DecorationType.Table);
        selectedNavBtn = decorNavButtonList[0];
        decorNavButtonList[0].SelectBtn(true);
    }

    public void SetSelectedNavBtn(DecorNavButton selectedNavBtn)
    {
        if(this.selectedNavBtn != selectedNavBtn)
        {
            this.selectedNavBtn.SelectBtn(false);
            this.selectedNavBtn = selectedNavBtn;
        }
    }

    public void InitDecorationData(DecorationType decorationType, bool force = false)
    {
        if(currentDecoration != decorationType || force)
        {
            this.currentDecoration = decorationType;
            DeactiveAllSlotUI();
            DecorationDataList dataList = ProfileManager.Instance.dataConfig.decorationDataConfig.GetDecorationDataList(currentDecoration);
            for (int i = 0; i < dataList.decorationDatas.Count; i++)
            {
                DecorSlotUI decorSlotUI = GetDecorSlotUI();
                decorSlotUI.InitSlot(currentDecoration, dataList.decorationDatas[i]);
            }
        }
    }

    public DecorSlotUI GetDecorSlotUI()
    {
        for (int i = 0; i < decorSlotUIs.Count; i++)
        {
            if (!decorSlotUIs[i].gameObject.activeSelf)
            {
                decorSlotUIs[i].gameObject.SetActive(true);
                return decorSlotUIs[i];
            }
        }
        DecorSlotUI decorSlotUI = Instantiate(decorSlotUIPrefab, decorSlotContainer);
        decorSlotUIs.Add(decorSlotUI);
        return decorSlotUI;
    }

    void DeactiveAllSlotUI()
    {
        for (int i = 0; i < decorSlotUIs.Count; i++)
        {
            decorSlotUIs[i].gameObject.SetActive(false);
        }
    }
    public void OnClose()
    {
        uiPanelShowUp.OnClose(UIManager.instance.ClosePanelDecorations);
    }

}
