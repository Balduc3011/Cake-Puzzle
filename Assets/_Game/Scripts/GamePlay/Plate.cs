using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] PlateIndex plateIndex;
    [SerializeField] Animator anim;
    public Transform pointStay;
    public Cake currentCake;
    public WayPointTemp wayPoint;

    public void SetPlateIndex(int x, int y) {
        plateIndex = new PlateIndex(x, y);
    }
    public PlateIndex GetPlateIndex() {  return plateIndex; }

    public void SetCurrentCake(Cake cake) { currentCake = cake; }

    public void CakeDone() { currentCake = null; }

    public void Active() {
        anim.SetBool("Active", true);
    }

    public void Deactive()
    {
        anim.SetBool("Active", false);
    }

    public int CalculatePoint(int cakeID)
    {
        int point = 6;
        for (int i = 0; i < currentCake.pieces.Count; i++)
        {
            if (currentCake.pieces[i].gameObject.activeSelf)
            {
                point--;          
            }
        }
        point -= currentCake.pieceCakeID.Count - 1;
        return point;

    }
}
[System.Serializable]
public class PlateIndex {
    public int indexX;
    public int indexY;
    public PlateIndex(int x, int y) {  indexX = x; indexY = y;}
}
[System.Serializable]
public class WayPointTemp {
    public Plate nextPlate;
    public bool setDone;
}
