using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    [SerializeField] Transform mainCanvas;
    public GameObject backGround;
    Dictionary<UIPanelType, GameObject> listPanel = new Dictionary<UIPanelType, GameObject>();
    public bool isHasPopupOnScene = false;
    public bool isHasTutorial = false;
    public bool isHasShopPanel = false;
    public bool isHasPanelIgnoreTutorial = false;
    [SerializeField] RectTransform myRect;
    [SerializeField] GameObject ingameDebugConsole;
  
    public Camera mainCamera;
    public PanelTotal panelTotal;
    PanelBakery panelBakery;
    PanelDecorations panelDecorations;
    PanelShop panelShop;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
    }
    private void Start()
    {
        //uiPooling.FirstPooling();
        //StartCoroutine(WaitToSpawnUIPool());
        //ingameDebugConsole.SetActive(ProfileManager.Instance.IsShowDebug());
        ShowPanelTotal();
        ShowPanelLoading();
    }

    public bool isDoneFirstLoadPanel = false;
    bool isFirstLoad;
    public void FirstLoadPanel() {
        //LoadSceneManager.Instance.AddTotalProgress(30);
       
        isDoneFirstLoadPanel = true;
    }

    public void CloseAllPopUp(bool isFirstLoad = false) {
        isHasPopupOnScene = false;
        isHasShopPanel = false;
        foreach (KeyValuePair<UIPanelType, GameObject> panel in listPanel)
        {
            panel.Value.gameObject.SetActive(false);
        }
    }
    public bool IsHavePopUpOnScene() { return isHasPopupOnScene; }
    public bool IsHaveTutorial() { return isHasTutorial; }
    public void RegisterPanel(UIPanelType type, GameObject obj)
    {
        GameObject go = null;
        if (!listPanel.TryGetValue(type, out go))
        {
            //Debug.Log("RegisterPanel " + type.ToString());
            listPanel.Add(type, obj);
        }
        obj.SetActive(false);
    }
    public bool IsHavePanel(UIPanelType type) {
        GameObject panel = null;
        return listPanel.TryGetValue(type, out panel);
    }
    public GameObject GetPanel(UIPanelType type) {
        GameObject panel = null;
        if (!listPanel.TryGetValue(type, out panel)) {
            switch (type) {
                case UIPanelType.PanelBase:
                    panel = Instantiate(Resources.Load("UI/PanelBase") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelTotal:
                    panel = Instantiate(Resources.Load("UI/PanelTotal") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelSpin:
                    panel = Instantiate(Resources.Load("UI/PanelSpin") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelDailyReward:
                    panel = Instantiate(Resources.Load("UI/PanelDailyReward") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelItemsReward:
                    panel = Instantiate(Resources.Load("UI/PanelItemsReward") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelBakery:
                    panel = Instantiate(Resources.Load("UI/PanelBakery") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelPlayGame:
                    panel = Instantiate(Resources.Load("UI/PanelPlayGame") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelSetting:
                    panel = Instantiate(Resources.Load("UI/PanelSetting") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelDecorations:
                    panel = Instantiate(Resources.Load("UI/PanelDecorations") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelCakeReward:
                    panel = Instantiate(Resources.Load("UI/PanelCakeReward") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelLevelComplete:
                    panel = Instantiate(Resources.Load("UI/PanelLevelComplete") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelLoading:
                    panel = Instantiate(Resources.Load("UI/PanelLoading") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelShop:
                    panel = Instantiate(Resources.Load("UI/PanelShop") as GameObject, mainCanvas);
                    break;
            }
            if (panel) panel.SetActive(true);
            return panel;
        }
        return listPanel[type];
    }

    public void OpenBlockAll() {
        panelTotal.OpenObjBlockAll();
    }

    public void CloseBlockAll()
    {
        panelTotal.CloseObjBlockAll();
    }

    public void ShowPanelBase()
    {
        GameObject go = GetPanel(UIPanelType.PanelBase);
        go.SetActive(true);
    }

    public void ClosePanelBase() {
        GameObject go = GetPanel(UIPanelType.PanelBase);
        go.SetActive(false);
    }

    public void ShowPanelTotal()
    {
        GameObject go = GetPanel(UIPanelType.PanelTotal);
        go.SetActive(true);
        if(panelTotal ==  null)
        {
            panelTotal = go.GetComponent<PanelTotal>();
        }
    }

    public void ClosePanelTotal()
    {
        GameObject go = GetPanel(UIPanelType.PanelTotal);
        go.SetActive(false);
    }
    public void ShowPanelTotalContent()
    {
        CloseOtherMenu(UIPanelType.PanelTotal);
        if (panelTotal == null)
        {
            GameObject go = GetPanel(UIPanelType.PanelTotal);
            go.SetActive(true);
            panelTotal = go.GetComponent<PanelTotal>();
        }
        panelTotal.ShowMainSceneContent(true);
    }

    public void ShowPanelPlayGame()
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelPlayGame);
        go.SetActive(true);
        panelTotal.Transform.SetAsLastSibling();
    }
    public void ClosePanelPlayGame()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelPlayGame);
        go.SetActive(false);
    }


    public void ShowPanelSpin()
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelSpin);
        go.SetActive(true);
    }
    public void ClosePanelSpin()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelSpin);
        go.SetActive(false);
    }
    public void ShowPanelDailyReward()
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelDailyReward);
        go.SetActive(true);
    }
    public void ClosePanelDailyReward()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelDailyReward);
        go.SetActive(false);
    }

    public void ShowPanelItemsReward()
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelItemsReward);
        go.SetActive(true);
    }
    public void ClosePanelItemsReward()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelItemsReward);
        go.SetActive(false);
    }

    public void ShowPanelBakery()
    {
        CloseOtherMenu(UIPanelType.PanelBakery);
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelBakery);
        go.SetActive(true);
        panelTotal.ShowMainSceneContent(false);
        panelTotal.Transform.SetAsLastSibling();
        if(panelBakery == null)
        {
            panelBakery = go.GetComponent<PanelBakery>();
        }
    }
    public void ClosePanelBakery()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelBakery);
        go.SetActive(false);
    }

    void CloseOtherMenu(UIPanelType ignorePanel)
    {
        if(panelBakery != null && ignorePanel != UIPanelType.PanelBakery)
        {
            if(panelBakery.gameObject.activeSelf)
            {
                panelBakery.OnClose();
            }
        }
        if (panelDecorations != null && ignorePanel != UIPanelType.PanelDecorations)
        {
            if (panelDecorations.gameObject.activeSelf)
            {
                panelDecorations.OnClose();
            }
        }
        if (panelShop != null && ignorePanel != UIPanelType.PanelShop)
        {
            if (panelShop.gameObject.activeSelf)
            {
                panelShop.OnClose();
            }
        }
    }

    public void ShowPanelSetting()
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelSetting);
        go.SetActive(true);
    }
    public void ClosePanelSetting()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelSetting);
        go.SetActive(false);
    }

    public void ShowPanelDecorations()
    {
        CloseOtherMenu(UIPanelType.PanelDecorations);
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelDecorations);
        go.SetActive(true);
        panelTotal.ShowMainSceneContent(false);
        panelTotal.Transform.SetAsLastSibling();
        GameManager.Instance.decorationManager.StartCamera(true);
        if (panelDecorations == null)
        {
            panelDecorations = go.GetComponent<PanelDecorations>();
        }
    }
    public void ClosePanelDecorations()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelDecorations);
        go.SetActive(false);
        GameManager.Instance.decorationManager.StartCamera(false);
    }

    public void ShowPanelShop()
    {
        CloseOtherMenu(UIPanelType.PanelShop);
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelShop);
        go.SetActive(true);
        panelTotal.ShowMainSceneContent(false);
        panelTotal.Transform.SetAsLastSibling();
        if (panelShop == null)
        {
            panelShop = go.GetComponent<PanelShop>();
        }
    }
    public void ClosePanelShop()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelShop);
        go.SetActive(false);
    }

    public void ShowPanelCakeReward()
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelCakeReward);
        go.SetActive(true);
    }
    public void ClosePanelCakeReward()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelCakeReward);
        go.SetActive(false);
    }
    public void ShowPanelLevelComplete()
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelLevelComplete);
        go.SetActive(true);
    }
    public void ClosePanelLevelComplete()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelLevelComplete);
        go.SetActive(false);
    }

    public void ShowPanelLoading() {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelLoading);
        go.SetActive(true);
    }
    public void ClosePanelLoading() {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelLoading);
        go.SetActive(false);
    }
}
