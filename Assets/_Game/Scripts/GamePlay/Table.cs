using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<Plate> plates = new List<Plate>();
    Plate[,] plateArray = new Plate[5,4];

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
    void PlateCheck(PlateIndex plateIndex) {
        List<Plate> plateNeedCheck = new List<Plate>();
        plateNeedCheck.Add(plateArray[plateIndex.indexX + 1, plateIndex.indexY]);
        plateNeedCheck.Add(plateArray[plateIndex.indexX - 1, plateIndex.indexY]);
        plateNeedCheck.Add(plateArray[plateIndex.indexX, plateIndex.indexY + 1]);
        plateNeedCheck.Add(plateArray[plateIndex.indexX, plateIndex.indexY - 1]);

        for (int i = 0;i < plateNeedCheck.Count;i++)
        {

        }
    }


}
