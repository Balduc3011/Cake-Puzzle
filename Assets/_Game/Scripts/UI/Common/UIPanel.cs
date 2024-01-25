using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum UIPanelType {
    PanelBase,
    PanelTotal,
    PanelSpin,
    PanelDailyReward,
    PanelItemsReward,
    PanelBakery,
    PanelPlayGame,
    PanelSetting,
    PanelDecorations
}
public class UIPanel : MonoBehaviour {
    public bool isRegisterInUI = true;
    protected UIPanelType panelType;
    public UIPanelAnimOpenAndClose openAndCloseAnim;
    public RectTransform topRectBase;

    // Start is called before the first frame update
    public virtual void Awake() {
        if (isRegisterInUI) UIManager.instance.RegisterPanel(panelType, gameObject);
        CheckScreenObstacleBase();
    }

    void CheckScreenObstacleBase()
    {
        if(topRectBase == null)
        {
            return;
        }
        float screenRatio = (float)Screen.height / (float)Screen.width;
        if (screenRatio > 2.1f) // Now we got problem 
        {
            topRectBase.sizeDelta = new Vector2(0, -100);
            topRectBase.anchoredPosition = new Vector2(0, -50);
        }
        else
        {
            topRectBase.sizeDelta = new Vector2(0, 0);
            topRectBase.anchoredPosition = new Vector2(0, 0);
        }
    }
}

