using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCake : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image onIconImg;
    [SerializeField] Image offIconImg;
    [SerializeField] GameObject usingMarkObj;
    [SerializeField] GameObject lockingMarkObj;
    void Start()
    {
        button.onClick.AddListener(OnCakeClick);
    }

    void OnCakeClick()
    {
        Debug.Log("Click cake");
        if(onIconImg.gameObject.activeSelf) 
            onIconImg.transform.DOScale(1.2f, 0.25f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                onIconImg.transform.DOScale(1, 0.05f);
            });
        if (offIconImg.gameObject.activeSelf)
            offIconImg.transform.DOScale(1.2f, 0.25f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                offIconImg.transform.DOScale(1, 0.05f);
            });
    }
}
