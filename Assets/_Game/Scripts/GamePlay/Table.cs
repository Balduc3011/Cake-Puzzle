using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class Table : MonoBehaviour
{
    public List<Plate> plates = new List<Plate>();
    [SerializeField] List<Material> plateMaterial;
    AnimationCurve curveRotate;
    AnimationCurve curveMove;
    float timeRotate;
    float timeMove;
    Plate[,] plateArray = new Plate[5, 4];

    private void Start()
    {
        InitData();
    }

    #region Decoration

    public void SetPlateMatColor(List<Color> colors)
    {
        for (int i = 0; i < plateMaterial.Count; i++)
        {
            plateMaterial[i].color = colors[i];
        }
    }
    #endregion

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
    public List<Plate> mapWay = new List<Plate>();

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

            //if (bestPlate.CheckCakeIsDone(cakeID))
            //{
            //    bestPlate.DoneCake();
            //}
            ClearCakeDone();
            // check other cake
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
            if (ways.Count > 0)
            {
                //Debug.Log("call start move from check id sot lai");
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
            //Debug.Log("call back start move");
            StartMove(cakeID);
            return;
        }
        if (timeMove == 0) {
            timeMove = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetTimeMove();
            timeRotate = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetTimeRotate();
            curveRotate = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetCurveRotate();
            curveMove = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetCurveMove();
        }

        if (stepIndex < ways.Count)
            ways[stepIndex].Move(cakeID, curveRotate, curveMove, timeRotate, timeMove, stepIndex == ways.Count - 1, Move);
    }

    public void StartCreateWay()
    {

        bestPlate.wayPoint.setDone = true;
        currentPieces = 0;
        ways.Clear();
        mapWay.Clear();
        bestPlate.currentSpace = bestPlate.GetFreeSpace();
        SetNextWayPoint(bestPlate.GetPlateIndex());
        CreateWayAfterSetNextPoint();
    }
    int currentPlateIndex = 0;
    void CreateWayAfterSetNextPoint()
    {
        if (!CheckDoneCreateWayMove()) { 
            currentPlateIndex = 0;
            CreateWayLoop();
        }
    }
    bool CheckDoneCreateWayMove()
    {
        if (mapWay[mapWay.Count - 1].currentSpace == 0) return true;
        for (int i = 0; i < mapWay.Count-1; i++) {
            if (mapWay[i].currentPieceSame > 0)
                return false;
        }
        return true;
    }
    void CreateWayLoop() {
        while (currentPlateIndex < mapWay.Count - 1)
        {
            if (mapWay[currentPlateIndex].currentPieceSame > 0)
            {
                int wayCountLoop = 0;
                int pieceFree = mapWay[currentPlateIndex].wayPoint.nextPlate.currentSpace;
                int pieceSame = mapWay[currentPlateIndex].currentPieceSame;
                if (pieceFree >= pieceSame) wayCountLoop = pieceSame;
                else wayCountLoop = pieceFree;
                for (int i = 0; i < wayCountLoop; i++)
                {
                    CreateWay(mapWay[currentPlateIndex]);
                }
                mapWay[currentPlateIndex].currentPieceSame -= wayCountLoop;
                mapWay[currentPlateIndex].currentSpace += wayCountLoop;
                mapWay[currentPlateIndex].wayPoint.nextPlate.currentPieceSame += wayCountLoop;
                mapWay[currentPlateIndex].wayPoint.nextPlate.currentSpace -= wayCountLoop;

               
            }
            currentPlateIndex++;
        }
        CreateWayAfterSetNextPoint();
    }

    public void SetNextWayPoint(PlateIndex plateIndex) {

        if ((plateIndex.indexX + 1) < plateArray.GetLength(0))
            CheckPlateCondition(plateArray[plateIndex.indexX, plateIndex.indexY], plateArray[plateIndex.indexX + 1, plateIndex.indexY]);

        if ((plateIndex.indexX - 1) >= 0)
            CheckPlateCondition(plateArray[plateIndex.indexX, plateIndex.indexY], plateArray[plateIndex.indexX - 1, plateIndex.indexY]);

        if ((plateIndex.indexY + 1) < plateArray.GetLength(1))
            CheckPlateCondition(plateArray[plateIndex.indexX, plateIndex.indexY], plateArray[plateIndex.indexX, plateIndex.indexY + 1]);

        if ((plateIndex.indexY - 1) >= 0)
            CheckPlateCondition(plateArray[plateIndex.indexX, plateIndex.indexY], plateArray[plateIndex.indexX, plateIndex.indexY - 1]);

        mapWay.Add(plateArray[plateIndex.indexX, plateIndex.indexY]);

    }
    public List<Way> ways = new List<Way>();

    void CreateWay(Plate plateStart) {
        if (plateStart.wayPoint.nextPlate == null)
            return;
        Way newWay = new Way();
        newWay.plateCurrent = plateStart;
        newWay.plateGo = plateStart.wayPoint.nextPlate;
        ways.Add(newWay);
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
                plateSetNext.currentPieceSame = pieceSame;
                plateSetNext.currentSpace = plateSetNext.GetFreeSpace();
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
    int totalNeedRotate = 0;
    int currentRotateDone = 0;
    public void ClearCakeDone()
    {
        totalNeedRotate = 0;
        currentRotateDone = 0;
        for (int i = 0; i < plates.Count; i++)
        {
            if (plates[i].currentCake != null && plates[i] != bestPlate)
            {
                if (plates[i].currentCake.CakeIsNull())
                {
                    plates[i].ClearCake();
                    continue;
                }

                if (plates[i].currentCake.needRotateRightWay && !plates[i].currentCake.cakeDone)
                {
                    totalNeedRotate++;
                    plates[i].currentCake.RotateOtherPieceRight(RotateDone);
                }
                if (plates[i].currentCake.cakeDone)
                {
                    plates[i].DoneCake();
                }
            }
        }
        if (totalNeedRotate == 0)
            CallBackCheckOtherCakeOnMap();
    }

    void RotateDone() {
        currentRotateDone++;
        if (currentRotateDone == totalNeedRotate)
            CallBackCheckOtherCakeOnMap();
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

    int indexCakeClear = 0;
    public void ClearAllCakeByItem()
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

    public void ClearAllCake() {
        for (int i = 0; i < plates.Count; i++)
        {
            if (plates[i].currentCake != null)
            {
                Destroy(plates[i].currentCake.gameObject);
            }
        }
    }

    public void ActivePlate(int indexX, int indexY) {
        if (indexX >= 0 && indexX <= 5 && indexY >= 0 && indexY <= 3)
        {
            plateArray[indexX, indexY].ActiveByItem();
        }
    }

    public void ClearPlateByBomb(int indexX, int indexY) {
        if (indexX >= 0 && indexX <= 5 && indexY >= 0 && indexY <= 3)
        {
            plateArray[indexX, indexY].ClearCakeByBomb();
        }
    }

    public void DeActivePlate(int indexX, int indexY) {
        if (indexX >= 0 && indexX <= 5 && indexY >= 0 && indexY <= 3)
        {
            plateArray[indexX, indexY].DeActiveByItem();
        }
    }
    PlateIndex plateIndexTemp;
    public void LoadCakeOnPlate(Cake newCake, CakeOnPlate cakeOnPlate)
    {
        plateIndexTemp = cakeOnPlate.plateIndex;
        newCake.transform.parent = plateArray[plateIndexTemp.indexX, plateIndexTemp.indexY].pointStay.transform;
        newCake.transform.localPosition = Vector3.zero;
        plateArray[plateIndexTemp.indexX, plateIndexTemp.indexY].currentCake = newCake;
        newCake.InitData(cakeOnPlate.cakeIDs, plateArray[plateIndexTemp.indexX, plateIndexTemp.indexY]);
    }

    public void SaveCake() {
        for (int i = 0; i < plates.Count; i++)
        {
            ProfileManager.Instance.playerData.cakeSaveData.SaveCake(plates[i].GetPlateIndex(), plates[i].currentCake);
        }
    }

    List<Plate> currentPlateNeedResolve = new();

    public void ResetPharse() {
        currentPlateNeedResolve.Clear();
    } 

    public List<IDInfor> GetIDInfor()
    {
        if (currentPlateNeedResolve.Count == 0)
        {
            for (int i = 0; i < plates.Count; i++)
            {
                if (plates[i].currentCake != null && IsHaveASpace(plates[i].plateIndex))
                {
                    plates[i].SetCurrentIDInfors();
                    currentPlateNeedResolve.Add(plates[i]);
                }
            }
        }

        return currentPlateNeedResolve[UnityEngine.Random.Range(0, currentPlateNeedResolve.Count)].idInfors;
    }

    bool IsHaveASpace(PlateIndex plateIndex) {
        if (plateIndex.indexX - 1 >= 0)
        {
            if (plateArray[plateIndex.indexX - 1, plateIndex.indexY].currentCake == null)
                return true;
        }

        if (plateIndex.indexX + 1 < plateArray.GetLength(0))
        {
            if (plateArray[plateIndex.indexX + 1, plateIndex.indexY].currentCake == null)
                return true;
        }

        if (plateIndex.indexY - 1 >= 0)
        {
            if (plateArray[plateIndex.indexX, plateIndex.indexY-1].currentCake == null)
                return true;
        }

        if (plateIndex.indexY + 1 < plateArray.GetLength(1))
        {
            if (plateArray[plateIndex.indexX, plateIndex.indexY + 1].currentCake == null)
                return true;
        }

        return false;
    }
}

[System.Serializable]
public class Way {
    public Plate plateCurrent;
    public Plate plateGo;
    Piece pieces;
    float timeDelay;
    Cake cake;
    UnityAction<int> actionCallBackMove;
    int cakeIDCallBack;
    AnimationCurve curveRotate;
    AnimationCurve curveMove;
    float timeRotate;
    float timeMove;
    bool lastMove;
    int rotateIndexReturn;

    public void Move(int cakeID, AnimationCurve curveRotate, AnimationCurve curveMove, float timeRotate, float timeMove, bool lastMove, UnityAction<int> actionDone = null)
    {
        this.curveRotate = curveRotate;
        this.curveMove = curveMove;
        this.timeRotate = timeRotate;
        this.timeMove = timeMove;
        this.lastMove = lastMove;

        cakeIDCallBack = cakeID;
        actionCallBackMove = actionDone;

        int totalFreeSpace = plateGo.GetFreeSpace();
        if (totalFreeSpace == 0)
        {
            DoActionDone();
            return;
        }
        pieces = plateCurrent.GetPieceMove(cakeID);
        if (pieces == null)
        {
            Debug.Log("call done move by piece null!");
            CallDoneThatMove();
        }
        else
        {
            rotateIndexReturn = plateGo.currentCake.MakeRotateIndexForNewPiece(cakeID);
            ActionCallBackOnMoveOtherPiece();
        }

        
       
    }

    void CallDoneThatMove() {
        DOVirtual.DelayedCall(timeMove - .15f, () =>
        {
            if (plateGo == GameManager.Instance.cakeManager.table.bestPlate)
            {
                //if (pieces != null)
                    if (plateGo.CheckCakeIsDone(pieces.cakeID))
                    {
                        plateGo.DoneCake();
                    }
            }
        });
        

        if (lastMove)
        {
            plateCurrent.MoveDoneOfCake();
            timeDelay = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetTimeEachCake();
        }
        else timeDelay = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetTimeEachPiece();
        DOVirtual.DelayedCall(timeDelay, () =>
        {
            DoActionDone();
        });
    }

    void ActionCallBackOnMoveOtherPiece() {
        if (rotateIndexReturn < 0) rotateIndexReturn = 5;
        else if (rotateIndexReturn >= 6) rotateIndexReturn = 0;

        cake = plateGo.currentCake;
       
        pieces.currentRotateIndex = rotateIndexReturn;
        plateGo.AddPiece(pieces);

        plateGo.currentCake.StartRotateOtherPieceForNewPiece(() => {
            //Debug.Log("Rotate done start move");
            
            pieces.transform.parent = plateGo.currentCake.transform;
            pieces.transform.DOScale(Vector3.one, 0.25f);
            pieces.transform.DOMove(plateGo.pointStay.position, timeMove).SetEase(Ease.InQuad).OnComplete(() => {
                //cake.DoAnimImpact();
            });
            plateGo.currentCake.CheckCakeIsDone(pieces.cakeID);
            if (!plateGo.currentCake.cakeDone) DOVirtual.DelayedCall(timeMove - .15f, cake.DoAnimImpact);
            pieces.transform.DORotate(new Vector3(0, plateGo.currentCake.rotates[rotateIndexReturn], 0), timeRotate).SetEase(Ease.InQuad);
            CallDoneThatMove();
            //Debug.Log(rotateIndexReturn);
        });
        plateCurrent.CheckNullPieces();
    }

    void DoActionDone() { actionCallBackMove(cakeIDCallBack); }

    
}


