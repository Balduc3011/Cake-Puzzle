using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerTop : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI trophyAmount;
    int trophyRecord;
    int trophy;
    void OnEnable()
    {
        //trophyAmount.text = ProfileManager.Instance.playerData.playerResourseSave.trophy.ToString();
        trophy = ProfileManager.Instance.playerData.playerResourseSave.trophy;
        trophyRecord = ProfileManager.Instance.playerData.playerResourseSave.trophyRecord;
        trophyAmount.text = trophyRecord.ToString();
        StartCoroutine(RunText());
    }

    IEnumerator RunText()
    {
        yield return new WaitForEndOfFrame();
        if (trophyRecord < trophy)
        {
            trophyRecord++;
            trophyAmount.text = trophyRecord.ToString();
            StartCoroutine(RunText());
        }
    }
}
