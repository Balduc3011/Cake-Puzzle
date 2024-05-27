using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CakeManager : MonoBehaviour
{
    public List<GroupCake> cakesWait = new List<GroupCake>();
    public List<Transform> pointSpawnGroupCake = new List<Transform>();
    public List<Cake> cakeNeedCheck = new List<Cake>();
    public List<float> countCake = new List<float>();

    List<IDInfor> idInfor = new();
    List<IDInfor> idNeedResolves = new();
    List<IDInfor> idReturn = new();
    List<CakeOnWait> cakeOnWaits = new List<CakeOnWait>();
    List<CakeOnPlate> cakeOnPlates = new List<CakeOnPlate>();

    public Vector3 vectorOffset;
    public Vector3 vectorOffsetStreak;
    Vector3 mousePos;
    Vector3 currentPos;

    public float posYDefault;
    public float distance;

    public bool onCheckLooseGame;
    public bool onMove;

    bool haveMoreThan3Cake;
    bool onInitGroup;
    bool haveMoreCake;
    public bool onCheckCake = false;
    bool loaded = false;

    public Table table;
    public GroupCake currentGCake;
    public CakeShowComponent cakeShowComponent;
    [SerializeField] Cake cakePref;

    GroupCake groupCake;
    Cake currentCakeCheck;
    StreakEffect streakEffect;

    public int justUnlockedCake;
    public bool levelUp;

    int currentStreak = 0;
    int indexGroupCake;
    int cakeIDIndex = -1;
    int indexCakeCheck;
    int timeCheckCake;
    int countFaild;
    int countIDRemain = 0;
    int limitRandom = 0;
    int countCheckFaild;
    int totalPieceInIDInfor = 0;

    public Transform trashBin;
    [SerializeField] Transform pointStart;
    [SerializeField] Transform pointEnd;

    UnityAction<Cake> actionCallBack;

    private void Start()
    {
        EventManager.AddListener(EventName.ChangeLevel.ToString(), LevelUp);
        EventManager.AddListener(EventName.UpdateCakeOnPlate.ToString(), UpdateCake);
    }

    bool isFirstTimeMove;

    private void Update()
    {
        if (currentGCake != null && isFirstTimeMove) {
            if (Input.GetMouseButtonUp(0))
            {
                isFirstTimeMove = false;
                if (!onMove)
                {
                    Drop();
                }
                else {
                    currentGCake.DropFail();
                    currentGCake = null;
                }
                return;
            }
            if (Input.GetMouseButton(0))
            {
                mousePos = Input.mousePosition;
                mousePos.z = Vector3.Distance(currentGCake.transform.position, Camera.main.transform.position);
                currentPos = Camera.main.ScreenToWorldPoint(mousePos) + vectorOffset;
                currentPos.y = posYDefault;
                currentGCake.transform.position = currentPos;
            }
            else
            {
                currentGCake.DropFail();
                currentGCake = null;
            }
        }
    }
    public bool SetCurrentGroupCake(GroupCake gCake) {
        if (currentGCake != null) return false;
        mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(gCake.transform.position, Camera.main.transform.position);
        currentPos = Camera.main.ScreenToWorldPoint(mousePos) + vectorOffset;
        currentPos.y = posYDefault;
        gCake.transform.DOMove(currentPos, .1f).SetEase(Ease.InCirc).OnComplete(() => {
            currentGCake = gCake;
            isFirstTimeMove = true;
        });
        return true;
    }

    void Drop() {
        currentGCake?.Drop();
        currentGCake = null;
        levelUp = false;
    }

    public void InitGroupCake() {
        SetCountPieces();
        indexGroupCake = 0;
        onInitGroup = true;
        ResetStreak();
        StartCoroutine(IE_WaitToInitGroupCake());
    }


    IEnumerator IE_WaitToInitGroupCake() {
        while (indexGroupCake < 3)
        {
            groupCake = GameManager.Instance.objectPooling.GetGroupCake();
            cakesWait.Add(groupCake);
            groupCake.transform.position = pointSpawnGroupCake[indexGroupCake].position;
            if (indexGroupCake != 2) {
                if (NeedResolve())
                {
                    idInfor = GetIDInfor();
                    if (idInfor == null || idInfor.Count == 0 || idInfor[0].count == 0)
                        groupCake.InitData((int)countCake[indexGroupCake], pointSpawnGroupCake[indexGroupCake], indexGroupCake);
                    else
                        groupCake.InitData(idInfor, pointSpawnGroupCake[indexGroupCake], indexGroupCake);
                }
                else
                    groupCake.InitData((int)countCake[indexGroupCake], pointSpawnGroupCake[indexGroupCake], indexGroupCake);
            }
            else
                groupCake.InitData((int)countCake[indexGroupCake], pointSpawnGroupCake[indexGroupCake], indexGroupCake);
            ProfileManager.Instance.playerData.cakeSaveData.AddCakeWait(groupCake, indexGroupCake);
            yield return new WaitForSeconds(.25f);
            indexGroupCake++;
        }
        ResetPharse();
        onInitGroup = false;
        CheckLooseGame(true);
    }


    public void RemoveCakeWait(GroupCake gCake)
    {
        ProfileManager.Instance.playerData.cakeSaveData.RemoveCakeWait(gCake.groupCakeIndex);
        cakesWait.Remove(gCake);
    }

    public void CheckSpawnCakeGroup()
    {
        if (cakesWait.Count <= 0) { InitGroupCake(); }
    }

    public void RemoveAllCakeWait() {
        for (int i = cakesWait.Count - 1; i >= 0; i--)
        {
            Destroy(cakesWait[i].gameObject);
            cakesWait.RemoveAt(i);
        }
        InitGroupCake();
    }


    void SetCountPieces() {
        countCake.Clear();
        haveMoreCake = ProfileManager.Instance.playerData.cakeSaveData.IsHaveMoreThanThreeCake();
        countCake.Add(ProfileManager.Instance.dataConfig.rateDataConfig.GetRandomSlot(haveMoreCake) + 1);
        countCake.Add(ProfileManager.Instance.dataConfig.rateDataConfig.GetRandomSlot(haveMoreCake) + 1);
        countCake.Add(ProfileManager.Instance.dataConfig.rateDataConfig.GetRandomSlot(haveMoreCake) + 1);
    }

    public int GetPiecesTotal() {
        haveMoreThan3Cake = ProfileManager.Instance.playerData.cakeSaveData.IsHaveMoreThanThreeCake();
        return ProfileManager.Instance.dataConfig.rateDataConfig.GetTotalPieces(haveMoreThan3Cake);
    }

    public int GetTotalCakeID() {
        haveMoreThan3Cake = ProfileManager.Instance.playerData.cakeSaveData.IsHaveMoreThanThreeCake();
        return ProfileManager.Instance.dataConfig.rateDataConfig.GetTotalCakeID(haveMoreThan3Cake);
    }

    public void SetupCheckCake() {
        table.SaveCake();
        if (onCheckCake) return;
        ClearCakeCheckDone();
        indexCakeCheck = -1;
        timeCheckCake = 0;
        if (!onCheckCake)
        {
            //Debug.Log("on check cake set true");
            onCheckCake = true;
            timeCallCheckLooseGame = 0;
            CheckOtherCake();
            //CheckNextCake();
        }

    }
    Cake cakeCheckTemp;
    public void CheckOtherCake(Cake cake = null)
    {
        GameManager.Instance.objectPooling.CheckGroupCake();

        //if (cakeNeedCheck.Count > 0 && table.CheckStuck())
        //{
        //    cakeNeedCheck.RemoveAt(0);
        //}

        if (cakeNeedCheck.Count > 0)
        {
            cakeCheckTemp = cakeNeedCheck[0];
            cakeNeedCheck.RemoveAt(0);
            StartCheckCake(cakeCheckTemp, CheckOtherCake);
        }
        else
        {
            table.SaveCake();
            onCheckCake = false;
            StartCheckLoseGame();
            CheckSpawnCakeGroup();
        }

    }

    void CheckNextCake(Cake cake = null) {
        //Debug.Log("Check next Cake");
        indexCakeCheck++;
        GameManager.Instance.objectPooling.CheckGroupCake();
        if (indexCakeCheck < cakeNeedCheck.Count && cakeNeedCheck.Count > 0)
        {
            while (cakeNeedCheck[indexCakeCheck] == null)
            {
                cakeNeedCheck.RemoveAt(indexCakeCheck);
            }
            if (indexCakeCheck < cakeNeedCheck.Count)
                StartCheckCake(cakeNeedCheck[indexCakeCheck], CheckNextCake);
        }
        else
        {
            timeCheckCake++;
            //Debug.Log("on check cake set false");
            onCheckCake = false;
            if (timeCheckCake >= 2)
            {
                table.SaveCake();
                CheckSpawnCakeGroup();
                //Debug.Log("on check cake set false");
                onCheckCake = false;
                ClearCakeNeedCheck();
                AddCakeCheckDone(cake);
            }
            else
            {
                //Debug.Log("on check cake set false");
                onCheckCake = true;
                indexCakeCheck = -1;
                CheckNextCake();
            }
        }
    }

    public List<Cake> cakeCheckDone = new();
    void AddCakeCheckDone(Cake cake) {
        if (cake == null) return;
        if (!cakeCheckDone.Contains(cake))
        {
            Debug.Log("add cake done");
            cakeCheckDone.Add(cake);
            if (cakeNeedCheck.Count == 0)
            {
                Invoke("StartCheckLoseGame", 0f);
            }
        }
    }

    int timeCallAddCheckCake;
    void ResetCheckCake()
    {
        Debug.LogError("Loi roi dmm");
        cakeNeedCheck.Clear();
    }
    void ClearCakeCheckDone() { cakeCheckDone.Clear(); }

    public void AddCakeNeedCheck(Cake cake, UnityAction actionCallBackSameCake = null) {
        if (cakeNeedCheck.Contains(cake))
        {
            timeCallAddCheckCake++;
            if (timeCallAddCheckCake > 30)
                ResetCheckCake();
            Debug.Log("add same cake: "+cake.currentPlate);
            //Debug.Break();
            actionCallBackSameCake();
            return;
        }
        cakeNeedCheck.Add(cake);
        if (onCheckLooseGame) CancelCheckLooseCake();
      
    }

    void CancelCheckLooseCake() {
        Debug.Log("Cancel invoke");
        CancelInvoke("CheckLooseGame"); 

    }

    public void ClearCakeNeedCheck() { cakeNeedCheck.Clear(); }

    public void StartCheckCake(Cake cake, UnityAction<Cake> actionCallBack)
    {
        this.actionCallBack = actionCallBack;
        currentCakeCheck = cake;
        cakeIDIndex = -1;
        totalIDNeedCheck = cake.pieceCakeID.Count;
        idNeedCheckOnCake = new(cake.pieceCakeID);
        CheckOtherIDOfCake();
    }
    int totalIDNeedCheck = 0;
    List<int> idNeedCheckOnCake = new();
    public void CheckOtherIDOfCake() {
        if (idNeedCheckOnCake.Count > 0)
        {
            if (CheckHaveCakeID(idNeedCheckOnCake[0]))
            {
                //Debug.Log("==================================================================================================");
                //Debug.Log("CHECK ID: "+ currentCakeCheck.pieceCakeID[cakeIDIndex] + " plate: "+ currentCakeCheck.currentPlate);
                int idNeedCheck = idNeedCheckOnCake[0];
                idNeedCheckOnCake.RemoveAt(0);
                table.ClearMapPlate(idNeedCheck);
                table.AddFirstPlate(currentCakeCheck.currentPlate);
                table.CreateMapPlate(currentCakeCheck.currentPlate.GetPlateIndex(), idNeedCheck);
                table.FindPlateBest(idNeedCheck);
                table.StartCreateWay();
                table.StartMove(idNeedCheck);
            }
            else
            {
                CheckOtherIDOfCake();
            }
        }
        else
        {
            Debug.Log("Call back check loose game");
            actionCallBack(currentCakeCheck);
        }
    }
    public void CheckIDOfCake() {
        cakeIDIndex++;
        if (cakeIDIndex < totalIDNeedCheck)
        {
            if (totalIDNeedCheck > currentCakeCheck.pieceCakeID.Count)
            {
                cakeIDIndex--;
                totalIDNeedCheck = currentCakeCheck.pieceCakeID.Count;
            }
            if (CheckHaveCakeID(currentCakeCheck.pieceCakeID[cakeIDIndex]))
            {
                //Debug.Log("==================================================================================================");
                //Debug.Log("CHECK ID: "+ currentCakeCheck.pieceCakeID[cakeIDIndex] + " plate: "+ currentCakeCheck.currentPlate);
                
                table.ClearMapPlate(currentCakeCheck.pieceCakeID[cakeIDIndex]);
                table.AddFirstPlate(currentCakeCheck.currentPlate);
                table.CreateMapPlate(currentCakeCheck.currentPlate.GetPlateIndex(), currentCakeCheck.pieceCakeID[cakeIDIndex]);
                table.FindPlateBest(currentCakeCheck.pieceCakeID[cakeIDIndex]);
                table.StartCreateWay();
                table.StartMove(currentCakeCheck.pieceCakeID[cakeIDIndex]);
            }
            else
            {
                CheckIDOfCake();
            }
        }
        else
        {
            actionCallBack(currentCakeCheck);
        }
    }

    bool CheckHaveCakeID(int cakeID) {
        return currentCakeCheck.CheckHaveCakeID(cakeID);
    }

    public void SetOnMove(bool onMove) { this.onMove = onMove; }

    int timeCallCheckLooseGame = 0;
    public void StartCheckLoseGame()
    {
        timeCallCheckLooseGame++;
        //Debug.Log("Total time check on this pharse: "+timeCallCheckLooseGame);
        if (timeCallCheckLooseGame > 30)
        {
            cakeNeedCheck.Clear();
            return;
        }
        if (cakesWait.Count > 0) {
            if (!onCheckLooseGame)
            {
                CheckLooseGame(false);
            }
        }
    }
    
    void CheckLooseGame(bool isCheckOnInit = false) {
        if (cakesWait.Count == 0 || onInitGroup) return;
        Debug.Log("On check Loose Game: "+ DateTime.Now);
        onCheckLooseGame = true;
        countCheckFaild = isCheckOnInit ? 3 : cakesWait.Count;
        countFaild = 0;
        for (int i = 0; i < cakesWait.Count; i++)
        {  
            if (cakesWait[i].cake.Count == 1)
            {
                if (!table.CheckGroupOneAble())
                    countFaild++;
            }
            else
            {
                if (!table.CheckGroupTwoAble(cakesWait[i].cakePosition))
                    countFaild++;
            }
        }
        //Debug.Log(countFaild + " count check fail: " + countCheckFaild);
        if (countFaild == countCheckFaild && countFaild > 0)
        {
            UIManager.instance.TurnBlock(true);
            table.AnimLooseGame();
            DOVirtual.DelayedCall(2f, () =>
            {
                UIManager.instance.TurnBlock(false);
                UIManager.instance.ShowPanelLevelComplete(false);
                UIManager.instance.panelTotal.OutTimeEvent();
                table.AnimLooseGameOut();
            });
        }  
        onCheckLooseGame = false;
        DOTween.ClearCachedTweens();
    }

    public Mesh GetNewUnlockedCakePieceMesh()
    {
        return ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakePieceMesh(justUnlockedCake);
    }

    public void SetJustUnlockedCake(int cakeID) { justUnlockedCake = cakeID; }

    public Mesh GetNextUnlockedCakePieceMesh()
    {
        int nextUnlockCake = ProfileManager.Instance.dataConfig.levelDataConfig.GetLevel(ProfileManager.Instance.playerData.playerResourseSave.currentLevel).cakeUnlockID;
        return ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakePieceMesh(nextUnlockCake);
    }

    public void ClearCake()
    {
        ProfileManager.Instance.playerData.cakeSaveData.ClearAllCake();
        table.ClearAllCakeByItem();
        RemoveAllCakeWait();
    }

    void UpdateCake() {
        if (cakeOnPlates.Count == 0)
        cakeOnPlates = ProfileManager.Instance.playerData.cakeSaveData.cakeOnPlates;
    }

    public void TrashIn(UnityAction actioncallBack) {
        trashBin.DOMove(pointEnd.position, .25f).SetEase(Ease.InQuad).OnComplete(() => {
            actioncallBack();
        });
    }
    public void TrashOut(UnityAction actioncallBack)
    {
        trashBin.DOMove(pointStart.position, .25f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            actioncallBack();
        });
    }

    public void ClearAllCake()
    {
        levelUp = false;
        table.ClearAllCake();
        for (int i = 0; i < cakesWait.Count; i++)
        {
            Destroy(cakesWait[i].gameObject);
        }
        cakesWait.Clear();
        InitGroupCake();
    }

  
    public void PlayGame()
    {
        if (loaded)
            return;
        if (ProfileManager.Instance.playerData.cakeSaveData.IsHaveCakeWaitSave())
        {
            LoadCakeWaitData();
            CheckLooseGame(true);
        }
        else { 
            InitGroupCake();
        }

        if (ProfileManager.Instance.playerData.cakeSaveData.IsHaveCakeOnPlate())
        {
            LoadCakeOnPlate();
        }
        loaded = true;
    }
 
    void LoadCakeOnPlate() {
        cakeOnPlates = ProfileManager.Instance.playerData.cakeSaveData.cakeOnPlates;
        for (int i = 0; i < cakeOnPlates.Count; i++)
        {
            Cake newCake = Instantiate(cakePref);
            table.LoadCakeOnPlate(newCake, cakeOnPlates[i]);
        }
        SetupCheckCake();
    }

    public void LoadCakeCheat() {
        Cake newCake = Instantiate(cakePref);
        table.LoadCakeOnPlateCheat(newCake);
    }

    public bool CakeOnWait(GroupCake myGroupCake)
    {
        for (int i = 0; i < cakesWait.Count; i++)
        {
            if (cakesWait[i] == myGroupCake)
                return true;
        }
        return false;
    }

    void LoadCakeWaitData() {
        cakeOnWaits = ProfileManager.Instance.playerData.cakeSaveData.cakeOnWaits;
        indexGroupCake = 0;
        countCake.Clear();
        for (int i = 0;i < cakeOnWaits.Count;i++)
        {
            countCake.Add(cakeOnWaits[i].cakeSaves.Count);
        }
        StartCoroutine(IE_WaitToInitGroupCakeFromSave());
    }

    IEnumerator IE_WaitToInitGroupCakeFromSave()
    {
        while (indexGroupCake < 3)
        {
            if ((int)countCake[indexGroupCake] > 0)
            {
                groupCake = GameManager.Instance.objectPooling.GetGroupCake();
                cakesWait.Add(groupCake);
                groupCake.transform.position = pointSpawnGroupCake[indexGroupCake].position;
                groupCake.InitData((int)countCake[indexGroupCake], pointSpawnGroupCake[indexGroupCake], indexGroupCake, cakeOnWaits[indexGroupCake].cakeSaves);
                ProfileManager.Instance.playerData.cakeSaveData.AddCakeWait(groupCake, indexGroupCake);
                yield return new WaitForSeconds(.25f);
            }
            indexGroupCake++;
        }
    }

    void LevelUp() {
        onMove = true;
        int newCakeID = ProfileManager.Instance.dataConfig.levelDataConfig.GetCakeID(ProfileManager.Instance.playerData.playerResourseSave.currentLevel - 1);
      
        SetJustUnlockedCake(newCakeID);
        levelUp = true;
        ShowLevelUp();
        //Invoke("ShowLevelUp", 3.5f);
    }

    void ShowLevelUp()
    {
        if (justUnlockedCake != -1 && justUnlockedCake != 0)
        {
            UIManager.instance.ShowPanelCakeReward();
        }
        else
        {
            UIManager.instance.ShowPanelSelectReward();
        }
    }

    public void UsingReroll() {
        for (int i = 0; i < cakesWait.Count; i++)
        {
            cakesWait[i].UsingRerollItem();
        }

        InitGroupCake();
    }

    public bool NeedResolve() { return cakeOnPlates.Count >= 12; }

  
    public List<IDInfor> GetIDInfor() {
        if (idNeedResolves != null)
            idNeedResolves.Clear();
        if (idReturn != null)
            idReturn.Clear();
        idNeedResolves = table.GetIDInfor();
        countIDRemain = 0;
        if(idNeedResolves != null)
            if (idNeedResolves.Count > 0) {
                for (int i = 0; i < idNeedResolves.Count; i++)
                {
                    countIDRemain = 4 - CalculateTotalPieces();
                    if (countIDRemain <= 0)
                        break;
                    IDInfor newIDInfor = new();
                    newIDInfor.ID = idNeedResolves[i].ID;
                    limitRandom = (6 - idNeedResolves[i].count + 1) > 4 ? 4 : (6 - idNeedResolves[i].count + 1);
                    newIDInfor.count = UnityEngine.Random.Range(1, limitRandom);
                    if (newIDInfor.count > countIDRemain)
                        newIDInfor.count = countIDRemain;
                    if (newIDInfor.count == 0)
                        newIDInfor.count = 1;
                    idReturn.Add(newIDInfor);
                }
        }
        return idReturn;
    }
 
    int CalculateTotalPieces() {
        totalPieceInIDInfor = 0;
        for (int i = 0; i < idReturn.Count; i++)
        {
            totalPieceInIDInfor += idReturn[i].count;
        }
        return totalPieceInIDInfor;
    }

    public void ResetPharse() {
        table.ResetPharse();
    }

    public void ReInitData(int cakeID) {
        for (int i = 0; i < cakesWait.Count; i++)
        {
            cakesWait[i].ReInitData(cakeID);
        }
        table.ReInitCake(cakeID);
    }
    #region STREAK
    void ResetStreak()
    {
        currentStreak = 0;
    }
  
    public void AddStreak(Cake cakeStreak)
    {
        currentStreak++;
        if (currentStreak > 1)
        {
            streakEffect = GameManager.Instance.objectPooling.GetStreakEffect();
            if (cakeStreak != null)
                streakEffect.SettingMaterial(cakeStreak.pieceCakeID[0]);
            else streakEffect.SettingMaterial(0);

            streakEffect.ChangeText(currentStreak.ToString());
            streakEffect.transform.position = Camera.main.WorldToScreenPoint(cakeStreak.transform.position) + vectorOffsetStreak;
            streakEffect.gameObject.SetActive(true);
        }
    }
    #endregion

    #region Ads

    int cakeCount;
    public void AddCakeCount()
    {
        if(ProfileManager.Instance.playerData.playerResourseSave.currentLevel >= 3)
        {
            cakeCount++;
            if(cakeCount > ConstantValue.VAL_CAKECOUNT_ADS)
            {
                cakeCount = 0;
                GameManager.Instance.ShowInter();
            }
        }
    }

    #endregion
}
