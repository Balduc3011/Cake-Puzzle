using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SwitchButton : MonoBehaviour
{
    [SerializeField] Transform trsSwitch;
    [SerializeField] Transform pointDeactive;
    [SerializeField] Transform pointActive;
    [SerializeField] Image imgButton;
    [SerializeField] List<Sprite> sprButton;

    public void SetActive(bool active, float timeSwitch) {
        trsSwitch.DOMove(active ? pointActive.position : pointDeactive.position, timeSwitch);
        imgButton.sprite = active ? sprButton[1] : sprButton[0];
    }
}
