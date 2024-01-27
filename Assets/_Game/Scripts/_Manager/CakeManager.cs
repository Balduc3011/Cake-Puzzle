using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CakeManager : MonoBehaviour
{
    public List<GroupCake> cakesWait = new List<GroupCake>();
    public List<Transform> pointSpawnGroupCake = new List<Transform>();
    public Table table;
    public GroupCake currentGCake;
    public Vector3 vectorOffset;
    public float distance;
    Vector3 mousePos;
    Vector3 currentPos;
    bool haveMoreThan3Cake;
    public float posYDefault;
    public bool onMove;

    private void Start()
    {
        InitGroupCake();
    }

    private void Update()
    {
        if (currentGCake != null) {
            if (Input.GetMouseButtonUp(0))
            {
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
            mousePos = Input.mousePosition;
            mousePos.z = distance;
            currentPos = Camera.main.ScreenToWorldPoint(mousePos) + vectorOffset;
            currentPos.y = posYDefault;
            currentGCake.transform.position = currentPos;
        }
    }

    public void SetCurrentGroupCake(GroupCake gCake) { 
        currentGCake = gCake; 
    }

    void Drop() {
        currentGCake.Drop();
        currentGCake = null;
    }

    public int GetRandomPieces() {
        return 0;
    }

    GroupCake groupCake;
    public List<float> countCake = new List<float>();
    public void InitGroupCake() {
        SetCountPieces();
        for (int i = 0; i < 3; i++) {
            groupCake = GameManager.Instance.objectPooling.GetGroupCake();
            cakesWait.Add(groupCake);
            groupCake.transform.position = pointSpawnGroupCake[i].position;
            groupCake.InitData((int)countCake[i], pointSpawnGroupCake[i], i);
        }
    }

    public void RemoveCakeWait(GroupCake gCake)
    {
        cakesWait.Remove(gCake);
        if (cakesWait.Count <= 0) { InitGroupCake(); }
    }

    bool haveMoreCake;
    void SetCountPieces() {
        countCake.Clear();
        haveMoreCake = ProfileManager.Instance.playerData.cakeSaveData.IsHaveMoreThanThreeCake();
        for (int i = 0; i < pointSpawnGroupCake.Count; i++)
        {
            countCake.Add(ProfileManager.Instance.dataConfig.rateDataConfig.GetRandomSlot(haveMoreCake) + 1);
        }
        countCake.Sort((a, b) => CompareCountCake(a, b));
    }

    int CompareCountCake(float a, float b) {
        if (a < b) return 1;
        if (a > b) return -1;
        return 0;
    }

    public int GetPiecesTotal() {
        haveMoreThan3Cake = ProfileManager.Instance.playerData.cakeSaveData.IsHaveMoreThanThreeCake();
        return ProfileManager.Instance.dataConfig.rateDataConfig.GetTotalPieces(haveMoreThan3Cake);
    }

    public int GetTotalCakeID() {
        haveMoreThan3Cake = ProfileManager.Instance.playerData.cakeSaveData.IsHaveMoreThanThreeCake();
        return ProfileManager.Instance.dataConfig.rateDataConfig.GetTotalCakeID(haveMoreThan3Cake);
    }

    int cakeIDIndex = -1;
    Cake currentCakeCheck;
    UnityAction actionCallBack;
    public void StartCheckCake(Cake cake, UnityAction actionCallBack)
    {
        this.actionCallBack = actionCallBack;
        currentCakeCheck = cake;
        cakeIDIndex = -1;
        CheckIDOfCake();
    }

    public void CheckIDOfCake() {
        //Debug.Log("current id index: " + cakeIDIndex);
        cakeIDIndex++;
        if (cakeIDIndex < currentCakeCheck.pieceCakeID.Count)
        {
            if (CheckHaveCakeID(currentCakeCheck.pieceCakeID[cakeIDIndex]))
            {
                //Debug.Log("=============START CHECK ID: " + currentCakeCheck.pieceCakeID[cakeIDIndex] + "==============");
                //Debug.Log(currentCakeCheck.pieceCakeID.Count);
                //Debug.Log("current id index: "+ cakeIDIndex);
                //Debug.Log(currentCakeCheck.currentPlate);
                table.ClearMapPlate(currentCakeCheck.pieceCakeID[cakeIDIndex]);
                table.AddFirstPlate(currentCakeCheck.currentPlate);
                table.CreateMapPlate(currentCakeCheck.currentPlate.GetPlateIndex(), currentCakeCheck.pieceCakeID[cakeIDIndex]);
                table.FindPlateBest(currentCakeCheck.pieceCakeID[cakeIDIndex]);
                table.StartCreateWay();
                table.StartMove(currentCakeCheck.pieceCakeID[cakeIDIndex]);
            }
            else
            {
                //Debug.Log("Check cake by call next cake");
                actionCallBack();
            }
        }
        else
        {
            //Debug.Log("Check cake by call next cake");
            actionCallBack();
        }
    }

    bool CheckHaveCakeID(int cakeID) {
        return currentCakeCheck.CheckHaveCakeID(cakeID);
    }

    public void SetOnMove(bool onMove) { this.onMove = onMove; }

    int countFaild;
    public void CheckLoseGame()
    {
        countFaild = 0;
        for (int i = 0; i < cakesWait.Count; i++)
        {
            if (cakesWait[i].cake.Count == 1)
            {
                if (!table.CheckGroupOneAble())
                countFaild++;
            }
            else {
                if(!table.CheckGroupTwoAble(cakesWait[i].cakePosition))
                    countFaild++;
            }
        }
        if (countFaild == cakesWait.Count) Debug.Log("Loose game");
        //Debug.Log( countFaild == cakesWait.Count);
    }
}
