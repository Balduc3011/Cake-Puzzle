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
    //bool firstPlay = true;
    [SerializeField] List<CardMoving> cardMovings;
    public override void Awake()
    {
        panelType = UIPanelType.PanelLoading;
        base.Awake();
    }

    private void OnEnable()
    {
        //firstLoadBG.SetActive(firstPlay);
        //firstPlay = false;
        transform.SetAsLastSibling();
        loadingBar.value = 0;
        SetCardMoving();
        DOVirtual.Float(0, 100, 3.5f, (value) =>
        {
            loadingBar.value = value;
            txtCurrentLoad.text = (int)value + "%";
        }).OnComplete(() => {
            UIManager.instance.ClosePanelLoading();
        });
    }

    void SetCardMoving()
    {
        for (int i = 0; i < cardMovings.Count; i++)
        {
            cardMovings[i].Move(Random.Range(0.5f, 0.75f));
        }
    }
}