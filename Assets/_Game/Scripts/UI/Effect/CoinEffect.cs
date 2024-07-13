using AssetKits.ParticleImage;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinEffect : MonoBehaviour
{
    public ParticleImage effect;
    
    public void Move(Transform target)
    {
        effect.attractorTarget = target;
        effect.Play();
        //tail.gameObject.SetActive(false);
        //vectorMoveTarget = transform.position;
        //vectorMoveTarget.y += offset;
        //transform.DOScale(1, .25f).From(.5f).SetEase(Ease.InBack);
        //transform.DOMove(vectorMoveTarget, .25f).SetEase(Ease.InSine).OnComplete(() => {
        //    tail.gameObject.SetActive(true);
        //    DOVirtual.DelayedCall(.5f, () => {
        //        effectMove.PrepareToMove(transform.position, target, () => {
        //            EffectAdd trsExpEffect = GameManager.Instance.objectPooling.GetEffectExp();
        //            trsExpEffect.transform.position = target.position;
        //            EventManager.TriggerEvent(EventName.ChangeCoin.ToString());
        //            Destroy(gameObject); 
        //        });
        //    });
        //});

    }

    public void CallCoinEffect() {
        UIManager.instance.panelTotal.coinBar.AnimChangeCoin();
    }
}
