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
}

[System.Serializable]
public class DecorationComponentInfo
{
    public DecorationType decorationType;
    public GameObject decorationObj;
    public Transform toMoveCamPosition;
    public float zoomSize;
}
