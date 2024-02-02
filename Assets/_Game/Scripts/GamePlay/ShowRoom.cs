using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoom : MonoBehaviour
{
    public GameObject showRoomCamera;
    public void ShowCamera() { 
        showRoomCamera.SetActive(true);
    }
    public void CloseCamera() {
        showRoomCamera.SetActive(false);
    }
}
