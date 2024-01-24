using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public CameraInfo mainCameraInfo;

    float currentSize;
    float widthScene;
    float heightScene;
    float persent;
    private void Awake()
    {
        currentSize = mainCamera.orthographicSize;
        widthScene  = Screen.width;
        heightScene = Screen.height;
        persent = 1080f / 1920f;
        persent = (widthScene / heightScene) / persent;
        mainCamera.orthographicSize = currentSize / persent;
    }

    public Camera GetMainCamera()
    {
        return mainCamera;
    }

    public void SwitchToMainCam()
    {
        TurnOffAllCamera();
        GetMainCamera().transform.DOMove(mainCameraInfo.position, 0.5f);
        GetMainCamera().DOOrthoSize(mainCameraInfo.zoomValue, 0.5f);
    }

    public void TurnOffAllCamera() {
        
    }
}

[System.Serializable]
public class CameraInfo
{
    public Vector3 position;
    public float zoomValue;
}
