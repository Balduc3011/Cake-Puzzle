using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionUI : MonoBehaviour
{
    [SerializeField] Transform objTrs;
    [SerializeField] Vector3 pinPos;
    [SerializeField] Vector3 movePos;
    [SerializeField] float moveDistance = 250f;
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] FloatDirection floatDirection;
    void Start()
    {
        pinPos = objTrs.transform.position;
        switch (floatDirection)
        {
            case FloatDirection.None:
                movePos = pinPos;
                break;
            case FloatDirection.Up:
                movePos = pinPos + new Vector3(0, moveDistance, 0);
                break;
            case FloatDirection.Down:
                movePos = pinPos - new Vector3(0, moveDistance, 0);
                break;
            case FloatDirection.Left:
                movePos = pinPos - new Vector3(moveDistance, 0, 0);
                break;
            case FloatDirection.Right:
                movePos = pinPos + new Vector3(moveDistance, 0, 0);
                break;
            default:
                movePos = pinPos;
                break;
        }
    }

    public void OnShow(bool show)
    {
        if(show)
        {
            objTrs.DOMove(pinPos, moveTime);
        }
        else
        {
            objTrs.DOMove(movePos, moveTime);
        }
    } 
}
