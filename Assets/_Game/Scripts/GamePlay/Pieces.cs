using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    public Transform parent;
    public GameObject objPref;
    public Vector3 objectOffset;

    public void InitData(GameObject objPref) { 
        this.objPref = objPref;
        if (parent.childCount > 0) { Destroy(parent.GetChild(0).gameObject); }
        Transform trs = Instantiate(objPref).transform;
        trs.parent = parent;
        trs.eulerAngles = new Vector3(90, 0, 0);
        trs.localPosition = objectOffset;
    }
}
