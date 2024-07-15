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
    //public GameObject backGround;
    Dictionary<UIPanelType, GameObject> listPanel = new Dictionary<UIPanelType, GameObject>();
    public bool isHasPopupOnScene = false;
    public bool isHasTutorial = false;
    public bool isHasShopPanel = false;
    public bool isHasPanelIgnoreTutorial = false;
    [SerializeField] RectTransform myRect;
    [SerializeField] GameObject ingameDebugConsole;
    [SerializeField] GameObject objBlock;
  
    public Camera mainCamera;
    public PanelTotal panelTotal;
    public PanelPlayGame panelGamePlay;
    PanelBakery panelBakery;
    PanelDecorations panelDecorations;
    PanelShop panelShop;
    PanelTopUp panelTopUp;

    [SerializeField] GameObject objSpawnPanel;
    [SerializeField] GameObject objEffect;
    [SerializeField] GameObject objCheat;
    [SerializeField] Button btnCheat;
    [SerializeField] Button btnShowUI;
    [SerializeField] Button btnTurnOffUI;

    public List<UIPanelType> onScene;
    public List<UnityAction> actionShowPanels = new();

    // Start is called before the first frame update
    void Awake() {
        instance = this;
        if (ProfileManager.Instance.versionStatus == VersionStatus.Cheat)
        {
            objCheat.SetActive(true);
            btnCheat.onClick.AddListener(ShowPanelCheat);
            btnTurnOffUI.onClick.AddListener(OffUI);
            btnShowUI.onClick.AddListener(ShowUI);
        }
        else
        {
            objCheat.SetActive(false);
        }
    }

    public void TurnBlock(bool active) {
        objBlock.SetActive(active);
    }

    void OffUI() {
        objSpawnPanel.SetActive(false);
        objEffect.SetActive(false);
        objCheat.SetActive(false);
        btnShowUI.gameObject.SetActive(true);
    }

    void ShowUI() {
        objSpawnPanel.SetActive(true);
        objEffect.SetActive(true);
        objCheat.SetActive(true);
        btnShowUI.gameObject.SetActive(false);
    }

    public void ShowPanelCheat() {
        isHasPopupOnScene = true;
        objCheat.SetActive(false);
        GameObject go = GetPanel(UIPanelType.PanelCheat);
        go.transform.SetAsLastSibling();
        go.SetActive(true);
    }
    public void ClosePanelCheat() {
        isHasPopupOnScene = false;
        objCheat.SetActive(true);
        GameObject go = GetPanel(UIPanelType.PanelCheat);
        go.SetActive(false);
    }
    private void Start()
    {
        //uiPooling.FirstPooling();
        //StartCoroutine(WaitToSpawnUIPool());
        if(ingameDebugConsole != null)
        ingameDebugConsole.SetActive(ProfileManager.Instance.IsShowDebug());
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
                case UIPanelType.PanelUsingItem:
                    panel = Instantiate(Resources.Load("UI/PanelUsingItem") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelLeaderBoard:
                    panel = Instantiate(Resources.Load("UI/PanelLeaderBoard") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelTopUp:
                    panel = Instantiate(Resources.Load("UI/PanelTopUp") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelQuickIAP:
                    panel = Instantiate(Resources.Load("UI/PanelQuickIAP") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelSelectReward:
                    panel = Instantiate(Resources.Load("UI/PanelSelectReward") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelHint:
                    panel = Instantiate(Resources.Load("UI/PanelHint") as GameObject, mainCanvas);
                    break;  
                case UIPanelType.PanelQuickTimeEvent:
                    panel = Instantiate(Resources.Load("UI/PanelQuickTimeEvent") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelDailyQuest:
                    panel = Instantiate(Resources.Load("UI/PanelDailyQuest") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelPreAds:
                    panel = Instantiate(Resources.Load("UI/PanelPreAds") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelCheat:
                    panel = Instantiate(Resources.Load("UI/CheatPanel") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelTutorial:
                    panel = Instantiate(Resources.Load("UI/PanelTutorial") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelLevelUp:
                    panel = Instantiate(Resources.Load("UI/PanelLevelUp") as GameObject, mainCanvas);
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

    public void ShowPanelTutorial()
    {
        GameObject go = GetPanel(UIPanelType.PanelTutorial);
        go.SetActive(true);
    }

    public void ClosePanelTutorial()
    {
        GameObject go = GetPanel(UIPanelType.PanelTutorial);
        go.SetActive(false);
    }

    public void ShowPanelHint(ItemType itemType)
    {
        if (itemType != ItemType.Hammer
            && itemType != ItemType.FillUp
            && itemType != ItemType.ReRoll
            )
            return;
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelHint);
        go.SetActive(true);
        go.GetComponent<PanelHint>().ShowComponent(itemType);
        go.transform.SetAsLastSibling();
    }

    public void ClosePanelHint() {
        isHasPopupOnScene = false;
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
        onScene.Remove(UIPanelType.PanelHint);
        GameObject go = GetPanel(UIPanelType.PanelHint);
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
        if (panelGamePlay == null)
        {
            GameObject go = GetPanel(UIPanelType.PanelPlayGame);
            panelGamePlay = go.GetComponent<PanelPlayGame>();
        }
        panelGamePlay.gameObject.SetActive(true);
        panelTotal.Transform.SetAsLastSibling();
    }
    public void ClosePanelPlayGame()
    {
        GameObject go = GetPanel(UIPanelType.PanelPlayGame);
        go.SetActive(false);
    }


    public void ShowPanelSpin()
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelSpin);
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelSpin);
            go.SetActive(true);
        }
    }
    public void ClosePanelSpin()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelSpin);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }
    public void ShowPanelDailyReward()
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelDailyReward);
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelDailyReward);
            go.SetActive(true);
        }
    }
    public void ClosePanelDailyReward()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelDailyReward);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
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
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }

    public void ShowPanelBakery(bool popUpOnScene = false)
    {
        if (isHasPopupOnScene && GameManager.Instance.playing)
        {
            actionShowPanels.Add(() =>
            {
                ShowPanelBakery(popUpOnScene);
            });
        }
        else
        {
            CloseOtherMenu(UIPanelType.PanelBakery);
            isHasPopupOnScene = popUpOnScene;
            GameObject go = GetPanel(UIPanelType.PanelBakery);
            go.SetActive(true);
            panelTotal.ShowMainSceneContent(false);
            panelTotal.Transform.SetAsLastSibling();
            if (panelBakery == null)
            {
                panelBakery = go.GetComponent<PanelBakery>();
            }
        }
    }
    public void ClosePanelBakery()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelBakery);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
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
        if (panelTopUp != null && ignorePanel != UIPanelType.PanelTopUp)
        {
            if (panelTopUp.gameObject.activeSelf)
            {
                panelTopUp.OnClose();
            }
        }
    }

    public void ShowPanelSetting()
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelSetting);
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelSetting);
            go.SetActive(true);
        }
    }
    public void ClosePanelSetting()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelSetting);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }

    public void ShowPanelDailyQuest()
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelDailyQuest);
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelDailyQuest);
            go.SetActive(true);
        }
    }
    public void ClosePanelDailyQuest()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelDailyQuest);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }

    public void ShowPanelDecorations(bool popUpOnScene = false)
    {
        if (isHasPopupOnScene && GameManager.Instance.playing)
        {
            actionShowPanels.Add(() =>
            {
                ShowPanelDecorations(popUpOnScene);
            });
        }
        else
        {
            CloseOtherMenu(UIPanelType.PanelDecorations);
            isHasPopupOnScene = popUpOnScene;
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
    }
    public void ClosePanelDecorations()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelDecorations);
        go.SetActive(false);
        GameManager.Instance.decorationManager.StartCamera(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }

    public void ShowPanelShop(bool popUpOnScene = false)
    {
        if (isHasPopupOnScene && GameManager.Instance.playing)
        {
            actionShowPanels.Add(() =>
            {
                ShowPanelShop(popUpOnScene);
            });
        }
        else
        {
            CloseOtherMenu(UIPanelType.PanelShop);
            isHasPopupOnScene = popUpOnScene;
            GameObject go = GetPanel(UIPanelType.PanelShop);
            go.SetActive(true);
            panelTotal.ShowMainSceneContent(false);
            panelTotal.Transform.SetAsLastSibling();
            if (panelShop == null)
            {
                panelShop = go.GetComponent<PanelShop>();
            }
        }
    }
    public void ClosePanelShop()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelShop);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
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
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }
    PanelLevelComplete panelLevelComplete;
    public void ShowPanelLevelComplete(bool isWinGame)
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(()=> {
                ShowPanelLevelComplete(isWinGame);
            });
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelLevelComplete);
            go.SetActive(true);
            if (panelLevelComplete == null) { panelLevelComplete = go.GetComponent<PanelLevelComplete>(); }
            panelLevelComplete.ShowPanel(isWinGame);
        }
    }
    public void ClosePanelLevelComplete()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelLevelComplete);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
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
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }

    public void ShowPanelUsingItem()
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelUsingItem);
        }
        else
        {
            //isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelUsingItem);
            go.SetActive(true);
        }
    }
    public void ClosePanelUsingItem()
    {
        //isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelUsingItem);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }

    public void ShowPanelLeaderBoard()
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelLeaderBoard);
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelLeaderBoard);
            go.SetActive(true);
        }
    }
    public void ClosePanelLeaderBoard()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelLeaderBoard);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }

    public void ShowPanelTopUp(bool popUpOnScene = false)
    {
        CloseOtherMenu(UIPanelType.PanelTopUp);
        isHasPopupOnScene = popUpOnScene;
        GameObject go = GetPanel(UIPanelType.PanelTopUp);
        go.SetActive(true);
        panelTotal.ShowMainSceneContent(false);
        panelTotal.Transform.SetAsLastSibling();
        if (panelTopUp == null)
        {
            panelTopUp = go.GetComponent<PanelTopUp>();
        }
    }

    public void ClosePanelTopUp()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelTopUp);
        go.SetActive(false);
    }

    public void ShowPanelQuickIAP(OfferID packageId)
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(()=> {
                ShowPanelQuickIAP(packageId);
            });
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelQuickIAP);
            go.SetActive(true);
            go.GetComponent<PanelQuickIAP>().Init(packageId);
        }
    }

    public void ClosePanelQuickIAP()
    {
        GameObject go = GetPanel(UIPanelType.PanelQuickIAP);
        go.transform.SetAsLastSibling();
        go.SetActive(false);
        isHasPopupOnScene = false;
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }
    public void ShowPanelSelectReward()
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelSelectReward);
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelSelectReward);
            go.SetActive(true);
        }
    }
    public void ClosePanelSelectReward()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelSelectReward);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }

    public void ShowPanelLevelUp()
    {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelLevelUp);
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelLevelUp);
            go.SetActive(true);
        }
    }
    public void ClosePanelLevelUp()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelLevelUp);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }

    public void ShowPanelQuickTimeEvent() {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelQuickTimeEvent);
        }
        else
        {
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelQuickTimeEvent);
            go.SetActive(true);
        }
    }

    public void ClosePanelQuickTimeEvent()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelQuickTimeEvent);
        go.SetActive(false);
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }
    
    public void ShowPanelPreAds() {
        if (isHasPopupOnScene)
        {
            actionShowPanels.Add(ShowPanelPreAds);
        }
        else
        {
            if (ProfileManager.Instance.versionStatus == VersionStatus.Cheat) return;
            isHasPopupOnScene = true;
            GameObject go = GetPanel(UIPanelType.PanelPreAds);
            go.SetActive(true);
        }
    }

    public void ClosePanelPreAds()
    {
        GameObject go = GetPanel(UIPanelType.PanelPreAds);
        go.SetActive(false);
        isHasPopupOnScene = false;
        if (actionShowPanels.Count > 0)
        {
            actionShowPanels[0]();
            actionShowPanels.RemoveAt(0);
        }
    }


}
