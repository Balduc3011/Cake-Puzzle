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

        for (int i = 0; i < cake.Count; i++)
        {
            if (cake[i].gameObject.activeSelf)
            {
                cake[i].DropDone();
            }
        }

        for (int i = 0; i < objConnects.Count; i++)
        {
            objConnects[i].SetActive(false);
        }
        GameManager.Instance.cakeManager.RemoveCakeWait(this);
        canTouch = false;
    }

    void DropFail() {
        for (int i = 0; i < cake.Count; i++)
        {
            if (cake[i].gameObject.activeSelf)
            {
                cake[i].GroupDropFail();
            }
        }
        transform.position = pointSpawn.position;
    }

    public void InitData(int countCake, Transform pointSpawn)
    {
        this.pointSpawn = pointSpawn;

        if (countCake == 2)
        {
            Init2Cakes();
        }
        else
        {

            for (int i = 0; i < countCake; i++)
            {
                cake[i].gameObject.SetActive(true);
                cake[i].InitData();
            }
          
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
    }

    void Init2Cakes() {

        cake[0].gameObject.SetActive(true);
        cake[0].InitData();

        int indexRandom = UnityEngine.Random.Range(1, cake.Count);
        cake[indexRandom].gameObject.SetActive(true);
        cake[indexRandom].InitData();

    }
}
