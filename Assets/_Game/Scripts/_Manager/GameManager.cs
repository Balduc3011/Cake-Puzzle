using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    public CameraManager cameraManager;
    public CakeManager cakeManager;
    public ObjectPooling objectPooling;
    //bool isTouching = false;
    //Vector3 touchUp, touchDown;
    //private void Update()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
    //        {
    //            return;
    //        }

    //    }
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (EventSystem.current.IsPointerOverGameObject())
    //        {
    //            return;
    //        }
    //        touchDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        isTouching = true;
    //    }
    //    if (isTouching)
    //    {
    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            isTouching = false;
    //            touchUp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;
    //            Physics.Raycast(ray, out hit);
    //            if (hit.collider != null)
    //            {
    //                //selectedRoom = hit.collider.GetComponent<RoomInterface>();
    //                if (hit.collider.gameObject.layer == 6)
    //                {
    //                    Debug.Log("slot");
    //                    //CameraMove.instance.ZoomOutCamera();
    //                    //UIManager.instance.ShowPanelUpgrade(selectedRoom);
    //                }
    //            }
    //        }
    //    }
    //}
}
