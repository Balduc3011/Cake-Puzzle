using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;

public class SwitchButton : MonoBehaviour
{
    [SerializeField] Transform trsSwitch;
    [SerializeField] Transform pointDeactive;
    [SerializeField] Transform pointActive;
    [SerializeField] Image imgButton;
    [SerializeField] List<Sprite> sprButton;
    [SerializeField] TextMeshProUGUI statusTxt;

    [SerializeField] Button changeStateBtn;
    UnityAction ChangeStateAction;

    string STATUS_ON = "ON";
    string STATUS_OFF = "OFF";

    private void Start()
    {
        changeStateBtn.onClick.AddListener(ChangeState);
    }

    public void SetActive(bool active, bool firstSet = false, float timeSwitch = 0.1f)
    {
        if (firstSet)
            trsSwitch.position = active ? pointActive.position : pointDeactive.position;
        else
            trsSwitch.DOMove(active ? pointActive.position : pointDeactive.position, timeSwitch);

        imgButton.sprite = active ? sprButton[1] : sprButton[0];
        statusTxt.text = active ? STATUS_ON : STATUS_OFF;
    }

    public void SetUp(UnityAction actionToSet)
    {
        ChangeStateAction = actionToSet;
    }

    void ChangeState()
    {
        if (ChangeStateAction != null)
        {
            ChangeStateAction();
        }
    }
}
