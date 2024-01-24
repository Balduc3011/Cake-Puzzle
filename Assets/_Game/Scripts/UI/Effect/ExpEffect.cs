using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtExpAdd;
    private void OnEnable()
    {
        DOVirtual.DelayedCall(1.5f, () => { gameObject.SetActive(false); });
    }
    public void ChangeTextExp(string exp) {
        txtExpAdd.text = "+" + exp + "EXP";
    }
}
