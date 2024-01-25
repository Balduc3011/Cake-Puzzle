using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCake : MonoBehaviour
{
    CakeData cakeData;
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
            onIconImg.transform.DOScale(1.2f, 0.25f).OnComplete(() =>
            {
                onIconImg.transform.DOScale(1, 0.05f);
            });
        if (offIconImg.gameObject.activeSelf)
            offIconImg.transform.DOScale(1.2f, 0.25f).OnComplete(() =>
            {
                offIconImg.transform.DOScale(1, 0.05f);
            });
        ProfileManager.Instance.playerData.cakeSave.UseCake(cakeData.id);
        UIManager.instance.GetPanel(UIPanelType.PanelBakery).GetComponent<PanelBakery>().ReloadPanel();
    }

    public void Init(CakeData cakeData)
    {
        this.cakeData = cakeData;
        onIconImg.sprite = cakeData.icon;
        offIconImg.sprite = cakeData.icon;
        InitUsing();
    }

    public void InitUsing()
    {
        if(ProfileManager.Instance.playerData.cakeSave.IsOwnedCake(cakeData.id))
        {
            lockingMarkObj.SetActive(false);
            offIconImg.gameObject.SetActive(false);
            onIconImg.gameObject.SetActive(true);
            usingMarkObj.SetActive(ProfileManager.Instance.playerData.cakeSave.IsUsingCake(cakeData.id));
        }
        else
        {
            lockingMarkObj.SetActive(true);
            offIconImg.gameObject.SetActive(true);
            onIconImg.gameObject.SetActive(false);
            usingMarkObj.SetActive(false);
        }
    }
}
