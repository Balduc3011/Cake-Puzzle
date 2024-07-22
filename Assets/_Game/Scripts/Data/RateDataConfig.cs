using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
[CreateAssetMenu(fileName = "RateDataConfig", menuName = "ScriptableObject/RateDataConfig")]
public class RateDataConfig : ScriptableObject
{
    public List<float> rateTotalSlot = new List<float>();
    public List<float> ratePiecesTotal = new List<float>();
    public List<float> rateCakeID = new List<float>();

    float randomResult;
    public float GetRandomSlot(bool moreThanThree, int currentTotalCake) {
        Debug.Log(" Current total Cake: " + currentTotalCake);
        if (currentTotalCake >= 12)
        {
            return 1;
        }
        randomResult = Random.Range(0, 100);
        for (int i = 0; i < rateTotalSlot.Count - 1; i++) {
            if (randomResult < rateTotalSlot[i])
            {
                return i;
            }
            else { 
                randomResult -= rateTotalSlot[i];
            }
        }
        if (moreThanThree) {
            return rateTotalSlot.Count - 1; }
        else return 0;
    }
    public int GetTotalPieces(bool moreThanThree) {
        randomResult = Random.Range(0, 100);
        for (int i = 0; i < ratePiecesTotal.Count - 1; i++)
        {
            if (randomResult < ratePiecesTotal[i])
            {
                return i;
            }
            else
            {
                randomResult -= ratePiecesTotal[i];
            }
        }
        if (moreThanThree) { return ratePiecesTotal.Count - 1; }
        else return 0;
    }
    public int GetTotalCakeID(bool moreThanThree) {
        randomResult = Random.Range(0, 100);
        for (int i = 0; i < rateCakeID.Count - 1; i++)
        {
            if (randomResult < rateCakeID[i])
            {
                return i;
            }
            else
            {
                randomResult -= rateCakeID[i];
            }
        }
        if (moreThanThree) { return rateCakeID.Count - 1; }
        else return 0;
    }
}
