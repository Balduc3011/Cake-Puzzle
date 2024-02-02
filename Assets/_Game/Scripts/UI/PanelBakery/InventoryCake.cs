using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCake : MonoBehaviour
{
    CakeData cakeData;
    [SerializeField] TextMeshProUGUI cakeNameTxt;
    [SerializeField] TextMeshProUGUI cakePointTxt;
    [SerializeField] Button button;
    [SerializeField] Image onIconImg;
    [SerializeField] Image offIconImg;
    [SerializeField] GameObject usingMarkObj;
    [SerializeField] GameObject lockingMarkObj;
    void Start()
    {
        button.onClick.AddListener(OnCakeClick);
    }

    private void OnEnable()
    {
        if(cakeData != null)
            InitUsing();
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
        ProfileManager.Instance.playerData.cakeSaveData.UseCake(cakeData.id);
        UIManager.instance.GetPanel(UIPanelType.PanelBakery).GetComponent<PanelBakery>().ReloadPanel();
    }

    public void Init(CakeData cakeData)
    {
        this.cakeData = cakeData;
        onIconImg.sprite = cakeData.icon;
        offIconImg.sprite = cakeData.icon;
        cakeNameTxt.text = "Cake " + cakeData.id.ToString();
        cakePointTxt.text = cakeData.id.ToString();
        InitUsing();
    }

    public void InitUsing()
    {
        if(ProfileManager.Instance.playerData.cakeSaveData.IsOwnedCake(cakeData.id))
        {
            lockingMarkObj.SetActive(false);
            offIconImg.gameObject.SetActive(false);
            onIconImg.gameObject.SetActive(true);
            usingMarkObj.SetActive(ProfileManager.Instance.playerData.cakeSaveData.IsUsingCake(cakeData.id));
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
