using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MoveManager : MonoBehaviour
{
    public List<MoveStep> moveSteps = new();
    bool onMove;

    public void AddMoveStep(Piece piece, Plate plateFrom, Plate plateTo, UnityAction actionCallBack = null) {
        MoveStep moveStep = new MoveStep();
        moveStep.pieceMove = piece;
        moveStep.plateFrom = plateFrom;
        moveStep.plateTo = plateTo;
        moveStep.actionCallBack = actionCallBack;
        moveStep.actionCallBackMoveManager = OnMove;
        moveSteps.Add(moveStep);
    }

    public void OnMove() {
        if (moveSteps.Count > 0)
        {
            moveSteps[0].OnMove();
        }
    }

    public void RemoveStep(MoveStep moveStep) { moveSteps.Remove(moveStep); }
}

public class MoveStep {
    public Piece pieceMove;
    public Plate plateFrom;
    public Plate plateTo;
    public UnityAction actionCallBack;
    public UnityAction actionCallBackMoveManager;

    public void OnMove() {
        if (actionCallBack == null)
        {
            
        }
    }
}
