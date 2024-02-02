using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationComponent : MonoBehaviour
{
    [SerializeField] Camera decorationCamera;
    [SerializeField] List<DecorationComponentInfo> decorations;

    public void StartCamera(bool start)
    {
        decorationCamera.gameObject.SetActive(start);
    }

    public void ShowComponent(DecorationType decorationType)
    {
        DecorationComponentInfo decorationComponentInfo = GetDecorationComponentInfo(decorationType);
        if(decorationComponentInfo != null)
        {
            decorationCamera.transform.DOMove(decorationComponentInfo.toMoveCamPosition.position, 0.5f);
            decorationCamera.transform.DORotate(decorationComponentInfo.toMoveCamPosition.eulerAngles, 0.5f, RotateMode.Fast);
            decorationCamera.DOOrthoSize(decorationComponentInfo.zoomSize, 0.5f);
        }
    }

    public DecorationComponentInfo GetDecorationComponentInfo(DecorationType decorationType)
    {
        for (int i = 0; i < decorations.Count; i++)
        {
            if (decorations[i].decorationType == decorationType) return decorations[i];
        }
        return null;
    }
}

[System.Serializable]
public class DecorationComponentInfo
{
    public DecorationType decorationType;
    public GameObject decorationObj;
    public Transform toMoveCamPosition;
    public float zoomSize;
}
