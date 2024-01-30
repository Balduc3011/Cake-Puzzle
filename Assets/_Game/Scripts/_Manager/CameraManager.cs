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

    float currentCamerasize;
    private void Awake()
    {
        currentSize = mainCamera.orthographicSize;
        widthScene = Screen.width;
        heightScene = Screen.height;
        persent = 1080f / 1920f;
        persent = (widthScene / heightScene) / persent;
        currentCamerasize = currentSize / persent;
        mainCamera.orthographicSize = currentCamerasize + 1f;
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

    public void FirstCamera() {
        
        DOVirtual.Float(mainCamera.orthographicSize, currentCamerasize, 1f, (value) =>
        {
            mainCamera.orthographicSize = value;
        });
    }
}

[System.Serializable]
public class CameraInfo
{
    public Vector3 position;
    public float zoomValue;
}
