using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class Table : MonoBehaviour
{
    public List<Plate> plates = new List<Plate>();
    Plate[,] plateArray = new Plate[5, 4];

    private void Start()
    {
        InitData();
    }

    void InitData() {
        int plateIndex = 0;
        for (int i = 0; i < plateArray.GetLength(0); i++)
        {
            for (int j = 0; j < plateArray.GetLength(1); j++) {
                plateArray[i, j] = plates[plateIndex];
                plates[plateIndex].SetPlateIndex(i, j);
                plateIndex++;
            }
        }
    }
    //List<Plate> plateNeedCheck = new List<Plate>();

    public List<Plate> mapPlate = new List<Plate>();

    // 0 => right
    // 1 => left
    // 2 => up
    // 3 => down

    public void ClearMapPlate(int cakeID) {
        ClearDoneSetWayPoint();
        mapPlate.Clear();
        currentCakeID = cakeID;
        stepIndex = -1; 
    }

    public void AddFirstPlate(Plate firstPlate) { mapPlate.Add(firstPlate); }
    int currentCakeID;
    public void CreateMapPlate(PlateIndex plateIndex, int cakeID) {
        List<Plate> plateNeedCheck = new List<Plate>();

        if ((plateIndex.indexX + 1) < plateArray.GetLength(0))
            plateNeedCheck.Add(plateArray[plateIndex.indexX + 1, plateIndex.indexY]);

        if ((plateIndex.indexX - 1) >= 0)
            plateNeedCheck.Add(plateArray[plateIndex.indexX - 1, plateIndex.indexY]);

        if ((plateIndex.indexY + 1) < plateArray.GetLength(1))
            plateNeedCheck.Add(plateArray[plateIndex.indexX, plateIndex.indexY + 1]);

        if ((plateIndex.indexY - 1) >= 0)
            plateNeedCheck.Add(plateArray[plateIndex.indexX, plateIndex.indexY - 1]);

        for (int i = 0; i < plateNeedCheck.Count; i++)
        {
            if (plateNeedCheck[i].currentCake == null)
                continue;
            if (plateNeedCheck[i].currentCake.GetCakePieceSame(cakeID))
            {
                if (!mapPlate.Contains(plateNeedCheck[i]))
                {
                    mapPlate.Add(plateNeedCheck[i]);
                    CreateMapPlate(plateNeedCheck[i].GetPlateIndex(), cakeID);
                }
            }
        }
    }

    public Plate bestPlate;
    int bestPoint;
    int totalPieceMerge;
    int currentPieces;
    int totalPieceMoveDone;
    public void FindPlateBest(int cakeID)
    {
        bestPoint = int.MinValue;
        for (int i = 0; i < mapPlate.Count; i++)
        {
            int points = mapPlate[i].CalculatePoint(cakeID);
            if (points > bestPoint) {
                bestPlate = mapPlate[i];
                bestPoint = points;
                totalPieceMerge = mapPlate[i].GetFreeSpace();
                totalPieceMoveDone = totalPieceMerge + mapPlate[i].GetCurrentPieceSame(cakeID);
            }
        }
    }

    public void StartMove(int cakeID) {
        if (CheckWayDone(cakeID))
        {
            if (bestPlate.CheckCakeIsDone(cakeID))
            {
                bestPlate.DoneCake();
            }
            ClearCakeDone();
            GameManager.Instance.cakeManager.CheckIDOfCake();
            return; 
        }
        stepIndex = -1;
        Move(cakeID);
    }
    bool CheckWayDone(int cakeID) {
        int totalDone = 0;
        for (int i = 0; i < ways.Count; i++)
        {
            if (ways[i].plateCurrent.CheckModeDone(cakeID))
                totalDone++;
        }
        return totalDone == ways.Count || bestPlate.BestPlateDone(cakeID, totalPieceMoveDone);
    }
    int stepIndex = -1;
    void Move(int cakeID) {
        stepIndex++;
        if (stepIndex == ways.Count)
        {
            StartMove(cakeID);
            return;
        }
        ways[stepIndex].Move(cakeID, Move);
    }

    public void StartCreateWay()
    {
       
        bestPlate.wayPoint.setDone = true;
        currentPieces = 0;
        ways.Clear();
        SetNextWayPoint(bestPlate.GetPlateIndex());
    }
    int piecesSame;
    public void SetNextWayPoint(PlateIndex plateIndex) {
       
        if ((plateIndex.indexX + 1) < plateArray.GetLength(0))
            CheckPlateCondition(plateArray[plateIndex.indexX, plateIndex.indexY], plateArray[plateIndex.indexX + 1, plateIndex.indexY]);

        if ((plateIndex.indexX - 1) >= 0)
            CheckPlateCondition(plateArray[plateIndex.indexX, plateIndex.indexY], plateArray[plateIndex.indexX - 1, plateIndex.indexY]);

        if ((plateIndex.indexY + 1) < plateArray.GetLength(1))
            CheckPlateCondition(plateArray[plateIndex.indexX, plateIndex.indexY], plateArray[plateIndex.indexX, plateIndex.indexY + 1]);

        if ((plateIndex.indexY - 1) >= 0)
            CheckPlateCondition(plateArray[plateIndex.indexX, plateIndex.indexY], plateArray[plateIndex.indexX, plateIndex.indexY - 1]);
        piecesSame = plateArray[plateIndex.indexX, plateIndex.indexY].currentPiecesCountGet;
        for (int i = 0; i < piecesSame; i++)
        {
            CreateWay(plateArray[plateIndex.indexX, plateIndex.indexY]);
        }
        
    }
    public List<Way> ways = new List<Way>();

    void CreateWay(Plate plateStart) {
        if (plateStart.wayPoint.nextPlate == null)
            return;
        Way newWay = new Way();
        newWay.plateCurrent = plateStart;
        newWay.plateGo = plateStart.wayPoint.nextPlate;
        ways.Add(newWay);
        if (plateStart.wayPoint.nextPlate != bestPlate)
            CreateWay(plateStart.wayPoint.nextPlate);
    }

    void CheckPlateCondition(Plate plateCurrent, Plate plateSetNext) {
       
        if (currentPieces >= totalPieceMerge) return;
        if (mapPlate.Contains(plateSetNext))
        {
            if (!plateSetNext.wayPoint.setDone)
            {
                plateSetNext.wayPoint.nextPlate = plateCurrent;
                plateSetNext.wayPoint.setDone = true;
                int pieceSame = plateSetNext.GetCurrentPieceSame(currentCakeID);
                if (pieceSame + currentPieces >= totalPieceMerge)
                {
                    pieceSame = totalPieceMerge - currentPieces;
                    currentPieces = totalPieceMerge;
                }
                else currentPieces += pieceSame;
                plateSetNext.SetCountPieces(pieceSame);
                SetNextWayPoint(plateSetNext.GetPlateIndex());
            }
        }
    }

    public void ClearDoneSetWayPoint() {
        for (int i = 0; i < mapPlate.Count; i++)
        {
            mapPlate[i].wayPoint.setDone = false;
            mapPlate[i].wayPoint.nextPlate = null;
        }
    }

    public void ClearCakeDone()
    {
        for (int i = 0; i < mapPlate.Count; i++)
        {
            if (mapPlate[i].currentCake != null)
                if (mapPlate[i].currentCake.cakeDone)
                {
                    mapPlate[i].ClearCake();
                }
        }
    }
}

[System.Serializable]
public class Way {
    public Plate plateCurrent;
    public Plate plateGo;
    bool moveDone;

    Piece pieces;
    public void Move(int cakeID, UnityAction<int> actionDone = null)
    {
        if (moveDone)
        {
            DoActionDone(cakeID, actionDone);
            return;
        }
        int totalFreeSpace = plateGo.GetFreeSpace();
        if (totalFreeSpace == 0)
        {
            DoActionDone(cakeID, actionDone);
            return;
        }
        pieces = plateCurrent.GetPieceMove(cakeID);
        if (pieces == null)
        {
            //moveDone = true;
        }
        else
        {
            int rotate = plateGo.currentCake.GetRotate(cakeID);
            pieces.transform.parent = plateGo.currentCake.transform;
            pieces.transform.localPosition = Vector3.zero;
            pieces.transform.eulerAngles = new Vector3(0, rotate, 0);
            plateGo.AddPiece(pieces);
        }
        plateCurrent.MoveDoneOfCake();
        DOVirtual.DelayedCall(.25f, () =>{
            if (actionDone != null)
            {
                actionDone(cakeID);
            }
        });
       
    }

    void DoActionDone(int cakeID, UnityAction<int> actionDone = null) { actionDone(cakeID); }
}

