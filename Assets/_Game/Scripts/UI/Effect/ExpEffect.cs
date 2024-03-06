using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpEffect : MonoBehaviour
{
    public TextMeshProUGUI txtExpAdd;
    public virtual void OnEnable()
    {
        DOVirtual.DelayedCall(1.5f, () => { gameObject.SetActive(false); });
    }
    public virtual void ChangeText(string exp) {
        txtExpAdd.text = "+" + exp + "EXP";
    }
}
