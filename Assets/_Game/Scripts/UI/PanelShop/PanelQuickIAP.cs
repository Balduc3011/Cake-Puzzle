using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelQuickIAP : UIPanel
{
    public List<IAPPopup> popups;
    [SerializeField] Button closeBtn;
    public override void Awake()
    {
        panelType = UIPanelType.PanelQuickIAP;
        base.Awake();
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClose);
    }

    public void Init(PackageId packageId)
    {
        for (int i = 0; i < popups.Count; i++)
        {
            if (popups[i].packageId == packageId)
            {
                popups[i].popUp.SetActive(true);
            }
            else
            {
                popups[i].popUp.SetActive(false);
            }
        }
    }

    void OnClose()
    {
        openAndCloseAnim.OnClose(UIManager.instance.ClosePanelQuickIAP);
    }
}

[System.Serializable]
public class IAPPopup
{
    public PackageId packageId;
    public GameObject popUp;
}
