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
    public override void Awake()
    {
        panelType = UIPanelType.PanelLoading;
        base.Awake();
    }

    private void OnEnable()
    {
        transform.SetAsLastSibling();
        loadingBar.value = 0;
        DOVirtual.Float(0, 100, 1.5f, (value) =>
        {
            loadingBar.value = value;
            txtCurrentLoad.text = (int)value + "%";
        }).OnComplete(() => {
            UIManager.instance.ClosePanelLoading();
        });
    }
}
