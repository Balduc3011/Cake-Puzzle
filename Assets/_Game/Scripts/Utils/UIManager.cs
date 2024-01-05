using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    [SerializeField] Transform mainCanvas;
    Dictionary<UIPanelType, GameObject> listPanel = new Dictionary<UIPanelType, GameObject>();
    public bool isHasPopupOnScene = false;
    public bool isHasTutorial = false;
    public bool isHasShopPanel = false;
    public bool isHasPanelIgnoreTutorial = false;
    [SerializeField] RectTransform myRect;
    [SerializeField] GameObject ingameDebugConsole;
  
    public Camera mainCamera;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
    }
    private void Start()
    {
        //uiPooling.FirstPooling();
        //StartCoroutine(WaitToSpawnUIPool());
        ingameDebugConsole.SetActive(ProfileManager.Instance.IsShowDebug());
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
            }
            if (panel) panel.SetActive(true);
            return panel;
        }
        return listPanel[type];
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

}
