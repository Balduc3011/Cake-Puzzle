using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITopup : MonoBehaviour
{
    public int slotId;
    [SerializeField] TextMeshProUGUI topName;
    [SerializeField] TextMeshProUGUI trophyAmount;
    Transform Transform;
    int runTurn;
    Vector3 startPos;
    Transform topPos;
    Transform botPos;
    int maxAmount = 7;
    int trophy;

    private void OnEnable()
    {
        topName.text = GameManager.Instance.GetRandomName();
        trophy = ProfileManager.Instance.playerData.playerResourseSave.trophy;
        trophy += (3 - slotId) * Random.Range(2, 4);
        trophyAmount.text = "---";
    }

    public void Setup(int id, Transform topPos, Transform botPos)
    {
        slotId = id;
        this.topPos = topPos;
        this.botPos = botPos;
        Transform = transform;
    }

    public void OnPlay()
    {
        runTurn = 0;
        startPos = Transform.position;
        StartRun();
    }

    void StartRun()
    {
        if(runTurn == 0)
            Transform.DOMove(botPos.position, 0.1f * (maxAmount - slotId)).SetEase(Ease.InOutQuad).OnComplete(StartRun);
        else
        {
            topName.text = GameManager.Instance.GetRandomName();
            Transform.position = topPos.position;
            if(runTurn < 2)
            {
                Transform.DOMove(botPos.position, 0.1f * maxAmount).SetEase(Ease.InOutQuad).OnComplete(StartRun);
            }
            else
            {
                Transform.DOMove(startPos, 0.1f * (slotId + 1)).SetEase(Ease.InOutQuad);
                trophyAmount.text = trophy.ToString();
            }
        }
        runTurn++;
    }
}
