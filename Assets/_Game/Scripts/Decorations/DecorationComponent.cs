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
        InitDecoration(DecorationType.Table);
        InitDecoration(DecorationType.Floor);
        InitDecoration(DecorationType.Plate);
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

    public void InitDecoration(DecorationType decorationType)
    {
        int usingDecorId = ProfileManager.Instance.playerData.decorationSave.GetUsingDecor(decorationType);
        DecorationComponentInfo decorationComponentInfo = GetDecorationComponentInfo(decorationType);
        if (decorationComponentInfo != null)
        {
            decorationComponentInfo.SetUpDecorationShow(usingDecorId);
        }
        if (decorationType != DecorationType.Table)
        {
            GameObject newDecor = GetDecorationObject(decorationType, usingDecorId);
        }
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
        if (decorationComponentInfo.objectDecoration.ContainsKey(decorId))
        {
            decorationComponentInfo.objectDecoration[decorId].SetActive(true);
            return decorationComponentInfo.objectDecoration[decorId];
        }
             
        return SpawnDecoration(decorationType, decorId);
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

    [SerializeField] Table table;
    public Dictionary<int, Color> tableColor = new Dictionary<int, Color>();

    public void SetUpDecorationShow(int currentId)
    {
        if(decorationType != DecorationType.Table)
        {
            int allPlateCount = ProfileManager.Instance.dataConfig.decorationDataConfig.GetDecorationDataList(decorationType).decorationDatas.Count;
            for (int i = 0; i < allPlateCount; i++)
            {
                if (objectDecoration.ContainsKey(i) && i != currentId)
                {
                    objectDecoration[i].SetActive(false);
                }
            }
        }
        else
        {
            List<Color> colors = ProfileManager.Instance.dataConfig.decorationDataConfig.GetDecorColor(decorationType, currentId);
            if (colors == null) return;
            if (colors.Count == 0) return;
            table.SetPlateMatColor(colors);
        }
    }
}

