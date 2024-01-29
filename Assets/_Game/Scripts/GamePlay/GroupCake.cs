using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupCake : MonoBehaviour
{
    public List<Cake> cake = new List<Cake>();
    [SerializeField] List<GameObject> objConnects = new List<GameObject>();

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
        if (onFollow) { 
            for (int i = 0;i < cake.Count;i++) {
                if (cake[i].gameObject.activeSelf) cake[i].CheckOnMouse();
            }
        }
    }

    public void OnFollowMouse() {
        if (canTouch)
        {
            GameManager.Instance.cakeManager.SetCurrentGroupCake(this);
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
        GameManager.Instance.cakeManager.RemoveCakeWait(this);
        canTouch = false;
        indexCake = -1;

        for (int i = 0; i < cake.Count; i++)
        {
            if (cake[i].gameObject.activeSelf)
            {
                if (i == cake.Count - 1)
                    cake[i].DropDone();
                else
                    cake[i].DropDone();
            }
        }
        DOVirtual.DelayedCall(.15f, CheckNextCake);
    }
    int indexCake;
    void CheckNextCake() {
        indexCake++;
        ClearCake();
        GameManager.Instance.objectPooling.CheckGroupCake();
        if (indexCake < cake.Count)
        {
            GameManager.Instance.cakeManager.SetOnMove(true);
            GameManager.Instance.cakeManager.StartCheckCake(cake[indexCake], CheckNextCake);
        }
        else {
            GameManager.Instance.cakeManager.SetOnMove(false);
            DOVirtual.DelayedCall(.5f, GameManager.Instance.cakeManager.CheckLoseGame);
        }
    }

    void ClearCake() {
        for (int i = cake.Count-1; i >= 0; i--)
        {
            if (cake[i] == null) cake.RemoveAt(i);
        }
    }

    public void DropFail() {
        for (int i = 0; i < cake.Count; i++)
        {
            if (cake[i].gameObject.activeSelf)
            {
                cake[i].GroupDropFail();
            }
        }
        transform.position = pointSpawn.position;
    }

    public void InitData(int countCake, Transform pointSpawn, int cakeWaitIndex)
    {
        this.pointSpawn = pointSpawn;
        if (cakeWaitIndex == 2) countCake = 1;
        if (countCake == 2)
        {
            Init2Cakes();
        }
        else
        {
            cake[0].gameObject.SetActive(true);
            cake[0].InitData();
        }

        objConnects[0].SetActive(cake[1].gameObject.activeSelf);
        objConnects[1].SetActive(cake[2].gameObject.activeSelf);

        //cakes2[1, 1].gameObject.SetActive(true);
        //cakes2[1, 1].InitData();

        //for (int i = 0; i < countCake - 1; i++)
        //{
        //    System.Random rd = new System.Random();
        //    int randomIndexX = (int)(rd.Next(cakes2.GetLength(0)));
        //    int randomIndexY = (int)(rd.Next(cakes2.GetLength(1)));
        //    cakes2[randomIndexX, randomIndexY].gameObject.SetActive(true);
        //    cakes2[randomIndexX, randomIndexY].InitData();
        //}

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
    void Init2Cakes() {

        cake[0].gameObject.SetActive(true);
        cake[0].InitData();

        int indexRandom = UnityEngine.Random.Range(1, cake.Count);
        cakePosition = indexRandom == 1 ? -1 : 1;
        cake[indexRandom].gameObject.SetActive(true);
        cake[indexRandom].InitData();

    }
    int countCakeDone;
    public void CheckGroupDone() {
        countCakeDone = 0;
        for (int i = 0; i < cake.Count; i++) {
            if (cake[i] == null || cake[i].cakeDone)
            {
                countCakeDone++;
            }
        }
        if (countCakeDone == cake.Count) {
            Destroy(gameObject);
        }
    }
}
