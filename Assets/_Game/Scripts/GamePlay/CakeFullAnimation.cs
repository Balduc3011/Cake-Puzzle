using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeFullAnimation : MonoBehaviour
{
    [SerializeField] float timeDoScale;
    [SerializeField] Vector3 scaleTo;
    [SerializeField] AnimationCurve curveScale;
    public void AnimDoneCake() {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(scaleTo, timeDoScale).SetEase(curveScale));
        sequence.Append(DOVirtual.DelayedCall(timeDoScale, () => { }));
        sequence.Append(transform.DOScale(Vector3.zero, timeDoScale));
        sequence.OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
