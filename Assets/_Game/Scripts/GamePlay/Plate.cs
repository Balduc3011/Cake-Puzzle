using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

public class Plate : MonoBehaviour
{
    [SerializeField] PlateIndex plateIndex;
    [SerializeField] Animator anim;
    public Transform pointStay;
    public Cake currentCake;
    public WayPointTemp wayPoint;
    public int currentPiecesCountGet;
    [SerializeField] Transform trsMove;
    [SerializeField] Vector3 pointMoveUp;
    [SerializeField] Vector3 pointMoveDown;
    private void Awake()
    {
        pointMoveUp = trsMove.position;
        pointMoveDown = trsMove.position;
        pointMoveUp.y += .3f;
    }
    public void SetPlateIndex(int x, int y) {
        plateIndex = new PlateIndex(x, y);
    }
    public PlateIndex GetPlateIndex() { return plateIndex; }

    public void SetCurrentCake(Cake cake) { currentCake = cake; }

    public void CakeDone() { currentCake = null; }

    public void Active() {
        //anim.SetBool("Active", true);
        trsMove.DOMove(pointMoveUp, .15f);
    }

    public void Deactive()
    {
        //anim.SetBool("Active", false);
        trsMove.DOMove(pointMoveDown, .15f);
    }

    public int CalculatePoint(int cakeID)
    {
        if (currentCake == null) return 0;
        int point = 0;
        for (int i = 0; i < currentCake.pieces.Count; i++)
        {
            if (currentCake.pieces[i].cakeID == cakeID)
            {
                point++;
            }
            else {
                point--;
            }
        }
        point += GetFreeSpace();
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
        if (currentCake == null) return true;
        return currentCake.CheckBestCakeDone(cakeID, totalPieceSame);
    }

    public int GetCurrentPieceSame(int cakeID)
    {
        if (currentCake == null) return 0;
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
        currentCake.RotateOtherPieceRightWay(0);
        if (currentCake.pieces.Count == 0) {
            //Destroy(currentCake.gameObject);
            //currentCake = null;
            currentCake.cakeDone = true;
        }
        else { currentCake.cakeDone = false;}
    }

    public void ClearCake() {
        Destroy(currentCake.gameObject);
        currentCake = null;
    }

    public bool CheckCakeIsDone(int cakeID) {
        if (currentCake == null) return true;
        return currentCake.CheckCakeIsDone(cakeID);
    }

    public void DoneCake()
    {
        if (currentCake != null)
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

    public void SetCountPieces(int countpieces) { currentPiecesCountGet = countpieces; }
    public void ResetPiecesCount() { currentPiecesCountGet = 0; }
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
