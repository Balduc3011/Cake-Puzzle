using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeFullAnimation : MonoBehaviour
{
    [SerializeField] float timeDoScale;
    [SerializeField] Vector3 scaleTo;
    [SerializeField] AnimationCurve curveScale;
    PanelTotal panelTotal;
    public void AnimDoneCake() {
        if (panelTotal == null)
        {
            panelTotal = UIManager.instance.GetPanel(UIPanelType.PanelTotal).GetComponent<PanelTotal>();
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(scaleTo, timeDoScale).SetEase(curveScale));
        sequence.Append(DOVirtual.DelayedCall(timeDoScale, () => { }));
        sequence.Append(transform.DOScale(Vector3.zero, timeDoScale));
        sequence.OnComplete(() => {
            EffectMove effectMove = GameManager.Instance.objectPooling.GetEffectMove();
            effectMove.gameObject.SetActive(true);
            effectMove.PrepareToMove(Camera.main.WorldToScreenPoint(transform.position), panelTotal.GetPointSlider(), () => {
                Transform trsExpEffect = GameManager.Instance.objectPooling.GetEffectExp();
                trsExpEffect.transform.position = panelTotal.GetPointSlider().position;
            });
            Destroy(gameObject);
        });
    }
}
