using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsingCake : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image onIconImg;
    [SerializeField] GameObject offIconObj;
    void Start()
    {
        button.onClick.AddListener(OnCakeClick);
    }

    void OnCakeClick()
    {

    }
}
