using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationComponent : MonoBehaviour
{
    [SerializeField] Camera decorationCamera;
    [SerializeField] List<DecorationComponentInfo> decorations;

    private void Start()
    {
        SpawnDecoration(DecorationType.Floor, 0);
        SpawnDecoration(DecorationType.Plate, 3);
    }

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

    public GameObject SpawnDecoration(DecorationType decorationType, int decorId)
    {
        DecorationComponentInfo decorationComponentInfo = GetDecorationComponentInfo(decorationType);
        GameObject newDecor = Instantiate(Resources.Load("Decoration/" + decorationType.ToString() + "/" + decorId.ToString()) as GameObject, decorationComponentInfo.spawnContainer);
        decorationComponentInfo.objectDecoration.Add(decorId, newDecor);
        return newDecor;
    }

    public GameObject GetDecorationObject(DecorationType decorationType, int decorId)
    {
        DecorationComponentInfo decorationComponentInfo = GetDecorationComponentInfo(decorationType);
        GameObject decorObj = decorationComponentInfo.objectDecoration[decorId];
        if (decorObj == null) 
        {
            decorObj = SpawnDecoration(decorationType, decorId);
        }
        return decorObj;
    }
}

[System.Serializable]
public class DecorationComponentInfo
{
    public DecorationType decorationType;
    public GameObject decorationObj;
    public Transform spawnContainer;
    public Transform toMoveCamPosition;
    public float zoomSize;
    public Dictionary<int, GameObject> objectDecoration = new Dictionary<int, GameObject>();

    
}
