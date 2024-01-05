using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFloating : MonoBehaviour
{
    Transform Transform;
    RectTransform rt;
    [SerializeField] Transform followTarget;
    [SerializeField] Vector3 offset;
    Camera cam;
    float counter;
    [SerializeField] Vector2 border;
    [SerializeField] RectTransform screenRect;
    public Vector2 screenSize;
    public Vector2 camControl;
    Vector2 cursorNone = new Vector2(0,0);
    Vector2 cursorLeft = new Vector2(-1,0);
    Vector2 cursorRight = new Vector2(1,0);
    Vector2 cursorUp = new Vector2(0,1);
    Vector2 cursorDown = new Vector2(0,-1);

    private void Awake()
    {
        //screenSize = new Vector2((float)Screen.width, (float)Screen.height);
        screenSize = new Vector2(screenRect.rect.width, screenRect.rect.height);
        rt = GetComponent<RectTransform>();
        Transform = transform;
    }

    private void OnEnable()
    {
        cam = GameManager.Instance.cameraManager.GetMainCamera();
    }

    // Update is called once per frame

    public void ResetPosition()
    {
        followTarget = null;
        rt.anchoredPosition = cursorNone;
    }
    public virtual void Update()
    {
        camControl = cursorNone;
        if (followTarget)
        {
            if(cam == null) cam = GameManager.Instance.cameraManager.GetMainCamera();
            Vector3 pos = cam.WorldToScreenPoint(followTarget.position + offset);
            if (Transform.position != pos)
            {
                Transform.position = pos;
                if(rt.anchoredPosition.x > screenSize.x / 2 - border.x / 2)
                {
                    rt.anchoredPosition = new Vector2(screenSize.x / 2 - border.x / 2, rt.anchoredPosition.y);
                    camControl = cursorRight;
                }
                if (rt.anchoredPosition.x < - screenSize.x / 2 + border.x / 2)
                {
                    rt.anchoredPosition = new Vector2(-screenSize.x / 2 + border.x / 2, rt.anchoredPosition.y);
                    camControl = cursorLeft;
                }

                if (rt.anchoredPosition.y > screenSize.y / 2 - border.y / 2)
                {
                    rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, screenSize.y / 2 - border.y / 2);
                    camControl = cursorUp;
                }
                if (rt.anchoredPosition.y < - screenSize.y / 2 + border.y / 2)
                {
                    rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, - screenSize.y / 2 + border.y / 2);
                    camControl = cursorDown;
                }
            }
            if(counter > 0)
            {
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    gameObject.SetActive(true);
                }
            }
        }
        ControlCam();
    }

    void ControlCam()
    {
        camControl = cursorNone;
        if (rt.anchoredPosition.x >= screenSize.x / 2 - border.x / 2)
        {
            camControl = cursorRight;
        }
        if (rt.anchoredPosition.x <= -screenSize.x / 2 + border.x / 2)
        {
            camControl = cursorLeft;
        }

        if (rt.anchoredPosition.y >= screenSize.y / 2 - border.y / 2)
        {
            camControl = cursorUp;
        }
        if (rt.anchoredPosition.y <= -screenSize.y / 2 + border.y / 2)
        {
            camControl = cursorDown;
        }
    }

    public virtual void ShowFloatingUI(Transform t)
    {
        //gameObject.SetActive(true);
        followTarget = t;
        gameObject.SetActive(false);
        counter = 0.05f;
    }

    public virtual void HideFloatingUI()
    {
        gameObject.SetActive(false);
        //gameObject.SetActive(false);
        camControl = cursorNone;
    }

    public void SetOffsetBySize(Vector3 size)
    {
        offset = new Vector3(2 - size.x , 1.5f, 2 - size.z);
    }

    public void SetTarget(Transform t)
    {
        followTarget = t;
    }
}
