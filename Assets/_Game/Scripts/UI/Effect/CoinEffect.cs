using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEffect : MonoBehaviour
{
    [SerializeField] float offset;
    Vector3 vectorMoveTarget;
    [SerializeField] EffectMove effectMove;
    public void Move(Transform target)
    {
        vectorMoveTarget = transform.position;
        vectorMoveTarget.y += offset;
        transform.DOScale(1, .25f).From(.5f).SetEase(Ease.InBack);
        transform.DOMove(vectorMoveTarget, .25f).SetEase(Ease.InSine).OnComplete(() => {
            DOVirtual.DelayedCall(.5f, () => {
                effectMove.PrepareToMove(transform.position, target, () => {
                    EffectAdd trsExpEffect = GameManager.Instance.objectPooling.GetEffectExp();
                    trsExpEffect.SetActionCallBack(() => {
                        EventManager.TriggerEvent(EventName.ChangeCoin.ToString());
                    });
                    trsExpEffect.transform.position = target.position;
                    Destroy(gameObject); 
                });
            });
        });
     
    }
}
