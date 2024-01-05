using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILook : MonoBehaviour
{
    public bool needUpdate;
    Camera mainCam;
    Vector3 VectorLoockAt;
    private void OnEnable()
    {
        OnUpdateAngle();
    }
    public void OnUpdateAngle() {
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam != null)
        {
            VectorLoockAt = mainCam.transform.position;
            VectorLoockAt.z *= -1;
            VectorLoockAt.y = transform.position.y;
            transform.LookAt(VectorLoockAt);
        }
    }
    private void Update()
    {
        if (needUpdate)
        {
            OnUpdateAngle();
        }
    }
}
