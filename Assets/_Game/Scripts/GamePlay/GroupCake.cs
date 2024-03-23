using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupCake : MonoBehaviour
{
    public List<Cake> cake = new List<Cake>();
    [SerializeField] List<GameObject> objConnects = new List<GameObject>();
    [SerializeField] List<Transform> pointDefaults;
    [SerializeField] List<Transform> pointMoveOnSelects;
    Transform pointDefault;
    Transform pointMove;
    public int groupCakeIndex;
    public float scaleDropFirst;
    public float scaleDropSeconds;
    public float timeDropScale08;
    public float timeDropScale1;
    //public Cake[,]cakes2 = new Cake[3, 3];

    Transform pointSpawn;

    bool canTouch = true;
    bool dropDone = false;
    bool onFollow = false;

    private void Awake()
    {
        AwakeInit();
    }

    public void AwakeInit() {
        for (int i = 0; i < cake.Count; i++)
        {
            cake[i].SetGroupCake(this);
            cake[i].gameObject.SetActive(false);
            cake[i].transform.position = pointDefaults[i].transform.position;
        }
        //int cakeIndex = 0;

        //for (int i = 0; i < cakes2.GetLength(0); i++) {
        //    for (int j = 0; j < cakes2.GetLength(1); j++) {
        //        cakes2[i, j] = cake[cakeIndex];
        //        cakes2[i, j].SetGroupCake(this);
        //        cakes2[i, j].gameObject.SetActive(false);
        //        cakeIndex++;
        //    }
        //}
    }

    private void Update()
    {
        if (onFollow && !GameManager.Instance.cakeManager.onCheckLooseGame) { 
            for (int i = 0;i < cake.Count;i++) {
                if (cake[i].gameObject.activeSelf) cake[i].CheckOnMouse();
            }
        }
    }

    public void OnFollowMouse() {
        if (canTouch)
        {
            GameManager.Instance.cakeManager.SetCurrentGroupCake(this);
            for (int i = 1; i < cake.Count; i++)
            {
                cake[i].transform.DOLocalMove(pointMove.localPosition, .25f).SetEase(Ease.InOutQuad);
            }

            for (int i = 0; i < cake.Count; i++)
            {
                cake[i].transform.DOScale(1.3f, .25f).SetEase(Ease.InOutQuad);
                cake[i].transform.DOScale(1.2f, .15f).SetEase(Ease.InOutQuad).SetDelay(.25f);
            }
            onFollow = true;
        }
    }

    public void Drop() {
        onFollow = false;
        for(int i = 0;i < cake.Count;i++) {
            if (cake[i].gameObject.activeSelf)
            {
                dropDone = cake[i].CheckDrop();
                if (!dropDone)
                {
                    DropFail();
                    return;
                }
            }
        }

        for (int i = 0; i < objConnects.Count; i++)
        {
            objConnects[i].SetActive(false);
        }
        canTouch = false;
        //indexCake = -1;

        for (int i = 0; i < cake.Count; i++)
        {
            //Debug.Log("index drop:"+i +" cake count: "+ cake.Count);
            cake[i].DropDone(i == cake.Count - 1, CallBackStartCheckCake);
            cake[i].transform.DOScale(scaleDropFirst, timeDropScale08).SetEase(Ease.InQuad);
            cake[i].transform.DOScale(scaleDropSeconds, timeDropScale1).SetEase(Ease.InOutQuad).SetDelay(timeDropScale08);

            GameManager.Instance.cakeManager.AddCakeNeedCheck(cake[i]);
        }
        //timeCheck = 0;
        GameManager.Instance.cakeManager.RemoveCakeWait(this);
       
       
    }

    void CallBackStartCheckCake() {
        GameManager.Instance.cakeManager.SetupCheckCake();
    }
    //int indexCake;
    //int timeCheck;
    //void CheckNextCake() {
    //    indexCake++;
    //    ClearCake();
    //    GameManager.Instance.objectPooling.CheckGroupCake();
    //    if (indexCake < cake.Count)
    //    {
    //        GameManager.Instance.cakeManager.SetOnMove(true);
    //        GameManager.Instance.cakeManager.StartCheckCake(cake[indexCake], CheckNextCake);
    //    }
    //    else {

    //        timeCheck++;
    //        if (timeCheck == 3)
    //        {
    //            GameManager.Instance.cakeManager.table.SaveCake();
    //            GameManager.Instance.cakeManager.SetOnMove(false);
    //            GameManager.Instance.cakeManager.RemoveCakeWait(this);
    //            GameManager.Instance.cakeManager.StartCheckLoseGame();
    //            GameManager.Instance.cakeManager.CheckSpawnCakeGroup();
    //        }
    //        else {
    //            indexCake = -1;
    //            CheckNextCake();
    //        }
    //    }
    //}

    public void DropFail() {
        for (int i = 0; i < cake.Count; i++)
        {
            if (cake[i].gameObject.activeSelf)
            {
                cake[i].GroupDropFail();
                if (i > 0)
                    cake[i].transform.DOLocalMove(pointDefault.localPosition, .2f).SetEase(Ease.InOutQuad);
                cake[i].transform.DOScale(1f, .25f).SetEase(Ease.InOutQuad);
            }
        }
        transform.position = pointSpawn.position;
    }

    public void InitData(int countCake, Transform pointSpawn, int cakeWaitIndex, List<CakeSave> cakeSaveDatas = null)
    {
        groupCakeIndex = cakeWaitIndex;
        this.pointSpawn = pointSpawn;
        if (cakeWaitIndex == 2) countCake = 1;
        if (countCake == 2)
        {
            Init2Cakes(cakeSaveDatas);
        }
        else
        {
            cake[0].gameObject.SetActive(true);
            if (cakeSaveDatas != null)
                cake[0].InitData(cakeSaveDatas[0]);
            else 
                cake[0].InitData();
        }

        objConnects[0].SetActive(cake[1].gameObject.activeSelf);
        objConnects[1].SetActive(cake[2].gameObject.activeSelf);

        if (objConnects[0].activeSelf)
        {
            pointDefault = pointDefaults[1];
            pointMove = pointMoveOnSelects[1];
            cake[0].transform.position = pointDefaults[0].position;
        }
        if (objConnects[1].activeSelf)
        {
            pointDefault = pointDefaults[2];
            pointMove = pointMoveOnSelects[2];
            cake[0].transform.position = pointMoveOnSelects[0].position;
        }



        ClearCakeNotUsing();
    }

    public void InitData(List<IDInfor> idInfors, Transform pointSpawn, int cakeWaitIndex) {
        groupCakeIndex = cakeWaitIndex;
        this.pointSpawn = pointSpawn;
        cake[0].gameObject.SetActive(true);
        ClearCakeNotUsing();
        cake[0].InitData(idInfors);
        objConnects[0].SetActive(false);
        objConnects[1].SetActive(false);
    }

    void ClearCakeNotUsing() {
        for (int i = cake.Count - 1; i >= 0; i--)
        {
            if (!cake[i].gameObject.activeSelf)
            {
                Destroy(cake[i].gameObject);
                cake.RemoveAt(i);
            }
        }
    }

    public int cakePosition;
    void Init2Cakes(List<CakeSave> cakeSaveDatas = null) {

        cake[0].gameObject.SetActive(true);
        

        int indexRandom = UnityEngine.Random.Range(1, cake.Count);
        cakePosition = indexRandom == 1 ? -1 : 1;
        cake[indexRandom].gameObject.SetActive(true);
        if (cakeSaveDatas != null)
        {
            cake[0].InitData(cakeSaveDatas[0]);
            cake[indexRandom].InitData(cakeSaveDatas[1]);
        }
        else {
            cake[0].InitData();
            cake[indexRandom].InitData();
        }
        

    }
    int countCakeDone;
    public void CheckGroupDone() {
        countCakeDone = 0;
        for (int i = 0; i < cake.Count; i++) {
            if (cake[i] == null || !cake[i].gameObject.activeSelf)
            {
                countCakeDone++;
            }
        }
        if (countCakeDone == cake.Count) {
            Destroy(gameObject);
        }
    }

    public void UsingRerollItem() {
        Transform trsEffect = GameManager.Instance.objectPooling.GetCakeDoneEffect();
        trsEffect.transform.position = transform.position;
        trsEffect.gameObject.SetActive(true);
        transform.DOScale(Vector3.zero, .25f).OnComplete(() => {
            GameManager.Instance.cakeManager.RemoveCakeWait(this);
        });
    }
}
