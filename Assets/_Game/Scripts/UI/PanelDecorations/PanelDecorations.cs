using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    DecorSlotUI selectedDecorSlot;
    [SerializeField] GameObject confirmObj;
    [SerializeField] CanvasGroup confirmCG;
    [SerializeField] Button confirmBuyBtn;
    [SerializeField] Button confirmCloseBtn;
    [SerializeField] TextMeshProUGUI priceTxt;
    [SerializeField] Image iconImg;
    public override void Awake()
    {
        panelType = UIPanelType.PanelDecorations;
        base.Awake();
        RawImageRect.sizeDelta = new Vector2(showingSize.rect.width, showingSize.rect.width);
        confirmBuyBtn.onClick.AddListener(OnConfirmBuyDecoration);
        confirmCloseBtn.onClick.AddListener(CloseConfirm);
    }

    private void OnEnable()
    {
        InitDecorationData(DecorationType.Table);
        InitSelectedTab();
        GameManager.Instance.decorationManager.ShowComponent(DecorationType.Table);
    }

    void InitSelectedTab()
    {
        for (int i = 0; i < decorNavButtonList.Count; i++)
        {
            decorNavButtonList[i].SelectBtn(false);
        }
        selectedNavBtn = decorNavButtonList[1];
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
            GameManager.Instance.decorationManager.ShowComponent(currentDecoration);
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



    public void ShowConfirm(DecorSlotUI decorSlotUI)
    {
        selectedDecorSlot = decorSlotUI;
        if(selectedDecorSlot != null)
        {
            confirmObj.SetActive(true);
            confirmCG.DOFade(1, 0.15f);
            priceTxt.text = selectedDecorSlot.priceTxt.text;
            iconImg.sprite = selectedDecorSlot.decorIconImg.sprite;
        }
        
    }

    public void CloseConfirm()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        confirmCG.DOFade(0, 0.15f).OnComplete(CloseConfirmInstant);
    }

    void CloseConfirmInstant()
    {
        confirmObj.SetActive(false);
    }

    void OnConfirmBuyDecoration()
    {
        if(selectedDecorSlot != null)
        {
            selectedDecorSlot.OnBuyConfirmed();
        }
        CloseConfirm();
    }
}
