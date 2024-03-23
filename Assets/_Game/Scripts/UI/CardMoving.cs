using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoving : MonoBehaviour
{
    public Transform card;
    public Transform start;
    public Transform target;

    public void Move(float delay = 0)
    {
        card.DOMove(target.position, 1f).From(start.position).SetDelay(delay).SetEase(Ease.OutQuart);
    }
}
