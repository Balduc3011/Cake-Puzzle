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

    public int GetFreeSpace() {
        int pieceSpace = 0;
        for (int i = 0; i < currentCake.pieces.Count; i++)
        {
            if (!currentCake.pieces[i].gameObject.activeSelf)
            {
                pieceSpace++;
            }
        }
        return pieceSpace;
    }

    public bool CheckModeDone(int cakeID) {
       return currentCake.CheckMoveDone(cakeID);
    }

    public bool BestPlateDone(int cakeID, int totalPieceSame) {
        return currentCake.CheckBestCakeDone(cakeID, totalPieceSame);
    }

    public int GetCurrentPieceSame(int cakeID)
    {
        return currentCake.GetCurrentPiecesSame(cakeID);
    }

    public void AddPiece(Pieces piece)
    {
        currentCake.AddPieces(piece);
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
