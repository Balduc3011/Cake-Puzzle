using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] AnimationCurve curvePosition, curveNoise;
    [SerializeField] float magnitudeRange = 1;
    [SerializeField] bool fixedMagnitude;
    Transform targetPoint;
    Vector3 startPoint;
    Transform trsMove;
    [SerializeField] bool needMove;
    float timeMove;
    Vector3 horizontalVector;
    float positionNoise;
    float magnitude;
    UnityAction actionDone;

    private void Start()
    {
        trsMove = transform;
    }

    public void PrepareToMove(Vector3 startPoint, Transform targetPoint, UnityAction actionDone = null)
    {
        this.startPoint = startPoint;
        this.targetPoint = targetPoint;
        if (fixedMagnitude) magnitude = magnitudeRange;
        else magnitude = Random.Range(-magnitudeRange, magnitudeRange);
        Vector2 direction = (Vector2)(targetPoint.position - startPoint);
        horizontalVector = Vector2.Perpendicular(direction);
        timeMove = 0f;
        if (actionDone != null)
            this.actionDone = actionDone;
        needMove = true;
    }

    public void Update()
    {
        if (needMove)
        {
            if (timeMove < curvePosition.keys[curvePosition.length - 1].time)
            {
                positionNoise = curveNoise.Evaluate(timeMove);
                trsMove.position = Vector2.Lerp(startPoint, targetPoint.position, curvePosition.Evaluate(timeMove))
                    + new Vector2(positionNoise * horizontalVector.x * magnitude, positionNoise * horizontalVector.y * magnitude);
                timeMove += Time.deltaTime * speed;
            }
            else
            {
                trsMove.position = targetPoint.position;
                if (actionDone != null)
                    actionDone();
                needMove = false;
                Destroy(gameObject);
            }
        }
    }
}
