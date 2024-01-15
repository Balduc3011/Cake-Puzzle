using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

    public int CalculatePoint()
    {
        if (currentCake == null) return 0;
        int point = 6;
        point -= currentCake.pieces.Count;
        point -= currentCake.pieceCakeID.Count - 1;
        return point;

    }

    public int GetFreeSpace() {
        if (currentCake == null) return 0;
        return 6 - currentCake.pieces.Count;
    }

    public bool CheckModeDone(int cakeID) {
        if (currentCake == null) return true;
        return currentCake.CheckMoveDone(cakeID);
    }

    public bool BestPlateDone(int cakeID, int totalPieceSame) {
        if (currentCake == null) return false;
        return currentCake.CheckBestCakeDone(cakeID, totalPieceSame);
    }

    public int GetCurrentPieceSame(int cakeID)
    {
        if (currentCake == null) return -1;
        return currentCake.GetCurrentPiecesSame(cakeID);
    }

    public void AddPiece(Piece piece)
    {
        currentCake.AddPieces(piece);
    }

    public void MoveDoneOfCake()
    {
        if (currentCake == null)
            return;
        if (currentCake.pieces.Count == 0) {
            Destroy(currentCake.gameObject);
            currentCake = null;
        }
    }

    public bool CheckCakeIsDone(int cakeID) {
        if (currentCake == null) return true;
        return currentCake.CheckCakeIsDone(cakeID);
    }

    public void DoneCake()
    {
        if (currentCake == null)
        {
            Destroy(currentCake.gameObject);
            currentCake = null;
        }
    }
    public Piece GetPieceMove(int cakeID) {
        if (currentCake == null)
            return null;
        return currentCake.GetPieceMove(cakeID);
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
