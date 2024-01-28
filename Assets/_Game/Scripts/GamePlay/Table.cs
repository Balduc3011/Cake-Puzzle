using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;
public class Table : MonoBehaviour
{
    public List<Plate> plates = new List<Plate>();
    [SerializeField] AnimationCurve curveRotate;
    [SerializeField] AnimationCurve curveMove;
    Plate[,] plateArray = new Plate[6, 4];

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
            if (points >= bestPoint) {
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
                //CallBackCheckOtherCakeOnMap();
            }
            ClearCakeDone();
            CallBackCheckOtherCakeOnMap();
            // check other cake

            //GameManager.Instance.cakeManager.CheckIDOfCake();


            return; 
        }
        stepIndex = -1;
        Move(cakeID);
    }

    void CallBackCheckOtherCakeOnMap() {
        ClearDoneSetWayPoint();
        ways.Clear();
        for (int i = mapPlate.Count - 1; i >= 0; i--) {
            if (mapPlate[i].currentCake == null || !mapPlate[i].currentCake.CheckHaveCakeID(currentCakeID))
            {
                mapPlate.RemoveAt(i);
            }
        }
        if (mapPlate.Count > 1)
        {
            Plate plateCheck = mapPlate[0];
            ClearMapPlate(currentCakeID);
            AddFirstPlate(plateCheck);
            CreateMapPlate(plateCheck.GetPlateIndex(), currentCakeID);
            ClearDoneSetWayPoint();
            FindPlateBest(currentCakeID);
            StartCreateWay();
            if (ways.Count>0)
            {
                StartMove(currentCakeID);
            }
            else
            {
                GameManager.Instance.cakeManager.CheckIDOfCake();
            }

        }
        else
        {
            GameManager.Instance.cakeManager.CheckIDOfCake();
        }
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
        ways[stepIndex].Move(cakeID, curveRotate, curveMove, Move);
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
            if (mapPlate[i].currentCake != null && mapPlate[i] != bestPlate)
            {
                mapPlate[i].currentCake.RotateOtherPieceRight(0);
                if (mapPlate[i].currentCake.cakeDone)
                {
                    mapPlate[i].ClearCake();
                }
            }
        }
    }

    public bool CheckGroupOneAble() {
        for (int i = 0; i < plates.Count; i++)
        {
            if (plates[i].currentCake == null)
                return true;
        }
        return false;
    }
    //positionSecondCake = -1 is top ; positionSecondCake = 1 is right
    public bool CheckGroupTwoAble(int positionSecondCake)
    {
        int pointXstart = 0;
        int pointYend = plateArray.GetLength(1);
        if (positionSecondCake == -1)
            pointXstart = 1;
        else pointYend--;

        for (int i = pointXstart; i < plateArray.GetLength(0); i++)
        {
            for (int j = 0; j < pointYend; j++)
            {
                if (positionSecondCake == -1)
                {
                    if (plateArray[i, j].currentCake == null && plateArray[i - 1, j].currentCake == null)
                    {
                        return true;
                    }
                }
                else {
                    if (plateArray[i, j].currentCake == null && plateArray[i, j + 1].currentCake == null)
                    {
                        return true;
                    }
                }
            }
        }
        return false;

    }

    int countTotalCakeCurrent = 0;
    int totalCakeReturn;
    public int GetCountID() {
        countTotalCakeCurrent = 0;
        totalCakeReturn = ProfileManager.Instance.playerData.cakeSaveData.cakeIDs.Count;
        for (int i = 0; i < plates.Count; i++)
        {
            if (plates[i].currentCake!=null)
            {
                countTotalCakeCurrent++;
            }
        }
        switch (countTotalCakeCurrent)
        {
            default:
                return 3;
        }
    }

    int indexCakeClear = 0;
    public void ClearAllCake()
    {
        indexCakeClear = 0;
        StartCoroutine(IE_WaitClearCake());
    }

    IEnumerator IE_WaitClearCake() {
       
        while (indexCakeClear < plates.Count)
        {
            if (plates[indexCakeClear].currentCake != null)
            {
                plates[indexCakeClear].ClearCakeFromItem();
                yield return new WaitForSeconds(.25f);
            }
            indexCakeClear++;
        }
        
        GameManager.Instance.cakeManager.TrashOut(UIManager.instance.CloseBlockAll);
    }

}

[System.Serializable]
public class Way {
    public Plate plateCurrent;
    public Plate plateGo;
    //bool moveDone;
    Vector3 vectorOffSet = new Vector3(0,1,0);
    Piece pieces;
    public void Move(int cakeID,AnimationCurve curveRotate, AnimationCurve curveMove, UnityAction<int> actionDone = null)
    {
        //if (moveDone)
        //{
        //    DoActionDone(cakeID, actionDone);
        //    return;
        //}
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
           
            int rotateIndex = plateGo.currentCake.GetRotateIndex(cakeID);
            pieces.transform.parent = plateGo.currentCake.transform;
            pieces.transform.DOMoveY(pieces.transform.position.y + 1f, .25f).OnComplete(() => {
                pieces.transform.DOMove(plateGo.pointStay.position, .25f).OnComplete(() => {
                    Transform trs = GameManager.Instance.objectPooling.GetPieceDoneEffect();
                    trs.position = pieces.transform.position + vectorOffSet;
                    trs.gameObject.SetActive(true);
                    
                });
            });
            
            pieces.transform.DORotate(new Vector3(0, plateGo.currentCake.rotates[rotateIndex], 0), .5f).SetEase(curveRotate);
            plateGo.AddPiece(pieces, rotateIndex);
        }
        plateCurrent.MoveDoneOfCake();
        DOVirtual.DelayedCall(.6f, () =>{
            if (actionDone != null)
            {
                actionDone(cakeID);
            }
        });
       
    }

    void DoActionDone(int cakeID, UnityAction<int> actionDone = null) { actionDone(cakeID); }
}

