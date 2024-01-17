using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Transform parent;
    public GameObject objPref;
    public Vector3 objectOffset;
    public int cakeID;
    public void InitData(GameObject objPref, int cakeID) { 
        this.objPref = objPref;
        this.cakeID = cakeID;
        if (parent.childCount > 0) { Destroy(parent.GetChild(0).gameObject); }
        Transform trs = Instantiate(objPref).transform;
        trs.parent = parent;
        //trs.eulerAngles = new Vector3(90, 0, 0);
        trs.localPosition = objectOffset;
    }
}
