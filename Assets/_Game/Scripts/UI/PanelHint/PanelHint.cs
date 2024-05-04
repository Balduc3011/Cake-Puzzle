using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHint : UIPanel
{
    public Button closeBtn;
    public List<HintComponent> hintComponents;
    public override void Awake()
    {
        panelType = UIPanelType.PanelHint;
        base.Awake();
        closeBtn.onClick.AddListener(OnClose);
    }

    public void ShowComponent(ItemType itemType)
    {
        for (int i = 0; i < hintComponents.Count; i++)
        {
            if (hintComponents[i].itemType == itemType)
            {
                hintComponents[i].component.SetActive(true);
            }
            else
            {
                hintComponents[i].component.SetActive(false);
            }
        }
    }

    void OnClose()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        openAndCloseAnim.OnClose(UIManager.instance.ClosePanelHint);
    }
}

[System.Serializable]
public class HintComponent
{
    public ItemType itemType;
    public GameObject component;
}