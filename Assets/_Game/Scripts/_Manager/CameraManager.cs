using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public CameraInfo mainCameraInfo;
    [SerializeField] Vector3 positionDefault;
    [SerializeField] Vector3 positionUsingItem;
    [SerializeField] Vector3 rotateCameraUsingItem;
    [SerializeField] Vector3 rotateCameraDefault;
    [SerializeField] Transform showRoomParents;
    [SerializeField] float cameraSizeUsingItem;
    [SerializeField] CameraShaker cameraShaker;

    Dictionary<int, ShowRoom> showRooms = new Dictionary<int, ShowRoom>();

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
        cameraSizeUsingItem = cameraSizeUsingItem / persent;
        mainCamera.orthographicSize = currentCamerasize + 1f;
        //cameraSizeUsingItem = currentCamerasize - 1f;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { UsingItemMode(); }
        if (Input.GetKeyDown(KeyCode.V)) { OutItemMode(); }
    }

    public void UsingItemMode() {
        mainCamera.transform.DOMove(positionUsingItem, .5f).SetEase(Ease.OutCubic);
        mainCamera.transform.DORotate(rotateCameraUsingItem, .5f).SetEase(Ease.OutCubic);
        DOVirtual.Float(mainCamera.orthographicSize, cameraSizeUsingItem, 1f, (value) =>
        {
            mainCamera.orthographicSize = value;
        }).SetEase(Ease.InOutSine);
       
    }

    public void OutItemMode()
    {
        mainCamera.transform.DOMove(positionDefault, .5f).SetEase(Ease.OutCubic);
        mainCamera.transform.DORotate(rotateCameraDefault, .5f).SetEase(Ease.OutCubic);
        DOVirtual.Float(mainCamera.orthographicSize, currentCamerasize, 1f, (value) =>
        {
            mainCamera.orthographicSize = value;
        }).SetEase(Ease.InOutSine);
    }
    ShowRoom showRoomTemp;
    ShowRoom currentShowRoom;
    public void ShowARoom(int showRoomIndex) {
        if (showRooms.TryGetValue(showRoomIndex, out showRoomTemp))
        {
            showRoomTemp = showRooms[showRoomIndex];
        }
        else
        {
            showRoomTemp = (Resources.Load("ShowRoom/ShowRoom_" + showRoomIndex) as GameObject).GetComponent<ShowRoom>();
            if (showRoomTemp != null)
            {
                ShowRoom newShowRoom = Instantiate(showRoomTemp, showRoomParents);
                showRooms.Add(showRoomIndex, newShowRoom);
                currentShowRoom = newShowRoom;
            }
        }
        showRoomTemp.gameObject.SetActive(true);
        currentShowRoom.ShowCamera();
        CloseMainCamera();
    }
    public void CloseMainCamera() {
        mainCamera.gameObject.SetActive(false);
    }

    public void OpenMainCamera()
    {
        mainCamera.gameObject.SetActive(true);
        if (currentShowRoom != null) currentShowRoom.CloseCamera();
    }

    public void ShowRoomCamera(bool show)
    {
        if (currentShowRoom != null)
        {
            if(show)
            {
                currentShowRoom.ShowCamera();
            }
            else
            {
                currentShowRoom.CloseCamera();
            }
        }
    }

    public void ShakeCamera(float duration) {
        cameraShaker.ShakeCamera(duration);
    }
}

[System.Serializable]
public class CameraInfo
{
    public Vector3 position;
    public float zoomValue;
}
