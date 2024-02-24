using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CakeAnimtionSetting", menuName = "ScriptableObject/CakeAnimtionSetting")]
public class CakeAnimtionSetting : ScriptableObject
{
    public AnimationCurve curveMove;
    public float timeMove;

    public AnimationCurve curveRotate;
    public float timeRotate;

    public AnimationCurve curveRotateRightWay;
    public float timeRotateRightWay;


    public float timeDelayEachPiece;
    public float timeDelayEachCake;

    public AnimationCurve GetCurveMove() { return curveMove; }
    public AnimationCurve GetCurveRotate() { return curveMove; }
    public AnimationCurve GetCurveRightWay() { return curveMove; }

    public float GetTimeMove() { return timeMove; }
    public float GetTimeRotate() { return timeRotate; }
    public float GetTimeRightWay() { return timeRotateRightWay; }
    public float GetTimeEachPiece() { return timeDelayEachPiece; }
    public float GetTimeEachCake() { return timeDelayEachCake; }
}
