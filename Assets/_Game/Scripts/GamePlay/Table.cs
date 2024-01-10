using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    public void ClearMapPlate() {
        mapPlate.Clear();
    }

    public void AddFirstPlate(Plate firstPlate) { mapPlate.Add(firstPlate); }

    public void CreateMapPlate(PlateIndex plateIndex, int cakeID, int plateIgnore = -1) {
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
                    //plateNeedCheck[i].wayPoint.nextPlate = plateArray[plateIndex.indexX, plateIndex.indexY];
                    CreateMapPlate(plateNeedCheck[i].GetPlateIndex(), cakeID, i);
                }
            }
        }
    }

    public Plate bestPlate;
    int bestPoint;
    public void FindPlateBest(int cakeID)
    {
        bestPoint = int.MinValue;
        for (int i = 0; i < mapPlate.Count; i++)
        {
            int points = mapPlate[i].CalculatePoint(cakeID);
            if (points > bestPoint) {
                bestPlate = mapPlate[i];
                bestPoint = points;
            }
        }
    }

    public void SetNextWayPoint(PlateIndex plateIndex) {

        if ((plateIndex.indexX + 1) < plateArray.GetLength(0))
        {
            if (mapPlate.Contains(plateArray[plateIndex.indexX + 1, plateIndex.indexY]))
            {
                if (!plateArray[plateIndex.indexX + 1, plateIndex.indexY].wayPoint.setDone)
                {
                    plateArray[plateIndex.indexX + 1, plateIndex.indexY].wayPoint.nextPlate = plateArray[plateIndex.indexX, plateIndex.indexY];
                    plateArray[plateIndex.indexX + 1, plateIndex.indexY].wayPoint.setDone = true;
                    SetNextWayPoint(plateArray[plateIndex.indexX + 1, plateIndex.indexY].GetPlateIndex());
                }
            }
           
        }
           

        if ((plateIndex.indexX - 1) >= 0)
        {
            if (mapPlate.Contains(plateArray[plateIndex.indexX - 1, plateIndex.indexY]))
            {
                if (!plateArray[plateIndex.indexX - 1, plateIndex.indexY].wayPoint.setDone)
                {
                    plateArray[plateIndex.indexX - 1, plateIndex.indexY].wayPoint.nextPlate = plateArray[plateIndex.indexX, plateIndex.indexY];
                    plateArray[plateIndex.indexX - 1, plateIndex.indexY].wayPoint.setDone = true;
                    SetNextWayPoint(plateArray[plateIndex.indexX - 1, plateIndex.indexY].GetPlateIndex());
                }
            }
        }
           
        

        if ((plateIndex.indexY + 1) < plateArray.GetLength(1))
        {
            if (mapPlate.Contains(plateArray[plateIndex.indexX, plateIndex.indexY + 1]))
            {
                if (!plateArray[plateIndex.indexX, plateIndex.indexY + 1].wayPoint.setDone)
                {
                    plateArray[plateIndex.indexX, plateIndex.indexY + 1].wayPoint.nextPlate = plateArray[plateIndex.indexX, plateIndex.indexY];
                    plateArray[plateIndex.indexX, plateIndex.indexY + 1].wayPoint.setDone = true;
                    SetNextWayPoint(plateArray[plateIndex.indexX, plateIndex.indexY + 1].GetPlateIndex());
                }
            }
        }


        if ((plateIndex.indexY - 1) >= 0)
        {
            if (mapPlate.Contains(plateArray[plateIndex.indexX, plateIndex.indexY - 1]))
            {
                if (!plateArray[plateIndex.indexX, plateIndex.indexY - 1].wayPoint.setDone)
                {
                    plateArray[plateIndex.indexX, plateIndex.indexY - 1].wayPoint.nextPlate = plateArray[plateIndex.indexX, plateIndex.indexY];
                    plateArray[plateIndex.indexX, plateIndex.indexY - 1].wayPoint.setDone = true;
                    SetNextWayPoint(plateArray[plateIndex.indexX, plateIndex.indexY - 1].GetPlateIndex());
                }
            }
        }
        
    }

    public void StartCreateWay()
    {
        bestPlate.wayPoint.setDone = true;
        SetNextWayPoint(bestPlate.GetPlateIndex());
    }

    public void ClearDoneSetWayPoint() {
        for (int i = 0; i < mapPlate.Count; i++)
        {
            mapPlate[i].wayPoint.setDone = false;
        }
    }
}
