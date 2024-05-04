using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecorNavButton : MonoBehaviour
{
    [SerializeField] Button navBtn;
    [SerializeField] TextMeshProUGUI navTxt;
    [SerializeField] DecorationType decorationType;
    [SerializeField] PanelDecorations panelDecorations;
    [SerializeField] Color activeColor;
    [SerializeField] Color deactiveColor;
    void Start()
    {
        navBtn.onClick.AddListener(NavBtnOnClick);
        navTxt.text = decorationType.ToString() + "s";
    }

    void NavBtnOnClick()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        panelDecorations.InitDecorationData(decorationType);
        panelDecorations.SetSelectedNavBtn(this);
        SelectBtn(true);
    }

    public void SelectBtn(bool select)
    {
        navTxt.color = select ? activeColor : deactiveColor;
    }

}
