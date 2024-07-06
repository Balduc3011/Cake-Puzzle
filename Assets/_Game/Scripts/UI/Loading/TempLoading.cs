using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempLoading : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] TextMeshProUGUI txtCurrentLoad;
    [SerializeField] List<CardMoving> cardMovings;

    private void Start()
    {
        SetCardMoving();
        DOVirtual.Float(0, 80, 1.5f, (value) =>
        {
            loadingBar.value = value;
            txtCurrentLoad.text = (int)value + "%";
        }).OnComplete(() => {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        });
    }

    void SetCardMoving()
    {
        for (int i = 0; i < cardMovings.Count; i++)
        {
            cardMovings[i].Move(true, 0.1f);
        }
    }
}
