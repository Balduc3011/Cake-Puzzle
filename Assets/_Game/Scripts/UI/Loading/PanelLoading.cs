using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelLoading : UIPanel
{
    [SerializeField] Slider loadingBar;
    [SerializeField] TextMeshProUGUI txtCurrentLoad;
    [SerializeField] CanvasGroup panelCanvas;

    [SerializeField] GameObject firstLoadBG;
    [SerializeField] GameObject logo;
    bool firstPlay = true;
    [SerializeField] List<CardMoving> cardMovings;
    public override void Awake()
    {
        panelType = UIPanelType.PanelLoading;
        base.Awake();
    }

    private void OnEnable()
    {
        firstLoadBG.SetActive(true);
        logo.SetActive(true);
        if (!firstPlay)
        {
            transform.SetAsLastSibling();
            loadingBar.value = 0;
            SetCardMoving(true);
            DOVirtual.Float(0, 100, 2f, (value) =>
            {
                loadingBar.value = value;
                txtCurrentLoad.text = (int)value + "%";
            }).OnComplete(() => {
                //firstLoadBG.SetActive(false);
                logo.SetActive(false);
                SetCardMoving(false);
                DOVirtual.DelayedCall(.5f, UIManager.instance.ClosePanelLoading);
            });
        }
        else
        {
            SetCardMoving(false);
            DOVirtual.Float(80, 100, 0.5f, (value) =>
            {
                loadingBar.value = value;
                txtCurrentLoad.text = (int)value + "%";
            }).OnComplete(() => {
                UIManager.instance.ClosePanelLoading();
            });
        }
        firstPlay = false;
    }

    void SetCardMoving(bool toScene)
    {
        for (int i = 0; i < cardMovings.Count; i++)
        {
            cardMovings[i].Move(toScene, toScene ? Random.Range(0.5f, 0.75f) : 0);
        }
    }
}