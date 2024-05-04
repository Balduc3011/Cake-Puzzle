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

    public void Init(OfferID packageId)
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
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        openAndCloseAnim.OnClose(UIManager.instance.ClosePanelQuickIAP);
    }
}

[System.Serializable]
public class IAPPopup
{
    public OfferID packageId;
    public GameObject popUp;
}
