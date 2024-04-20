using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Transform parent;
    public GameObject objPref;
    public Vector3 objectOffset;
    public int cakeID;
    public int currentRotateIndex;
    public void InitData(GameObject objPref, int cakeID, int rotateIndex) { 
        currentRotateIndex = rotateIndex;
        this.objPref = objPref;
        this.cakeID = cakeID;
        if (parent.childCount > 0) { Destroy(parent.GetChild(0).gameObject); }
        Transform trs = Instantiate(objPref).transform;
        trs.parent = parent;
        //trs.eulerAngles = new Vector3(90, 0, 0);
        trs.localPosition = objectOffset;
        trs.localScale = Vector3.one;
    }

    public void RemoveByFillUp(int index)
    {
        Debug.Log("Do Scale");
        transform.DOScale(Vector3.zero, .25f).SetDelay(index * .25f).SetEase(Ease.InBack).OnComplete(() => { 
            Destroy(gameObject);
        });
    }
}
