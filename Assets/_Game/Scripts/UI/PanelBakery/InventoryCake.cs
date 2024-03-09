using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCake : MonoBehaviour
{
    CakeData cakeData;
    OwnedCake currentCake;
    [SerializeField] TextMeshProUGUI cakeNameTxt;
    [SerializeField] TextMeshProUGUI cakePointTxt;
    [SerializeField] Button button;
    [SerializeField] Image onIconImg;
    [SerializeField] Image offIconImg;
    [SerializeField] GameObject usingMarkObj;
    [SerializeField] GameObject lockingMarkObj;
    string PERCAKE = "/cake";
    PanelBakery panelBakery;
    bool isUsing;

    [SerializeField] Slider cardAmountSlider;
    [SerializeField] TextMeshProUGUI cardAmountTxt;
    [SerializeField] TextMeshProUGUI levelTxt;
    void Start()
    {
        button.onClick.AddListener(OnCakeClick);
        panelBakery = UIManager.instance.GetPanel(UIPanelType.PanelBakery).GetComponent<PanelBakery>();
    }

    private void OnEnable()
    {
        if(cakeData != null)
            InitUsing();
    }

    void OnCakeClick()
    {
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
        //ProfileManager.Instance.playerData.cakeSaveData.UseCake(cakeData.id);
        //UIManager.instance.GetPanel(UIPanelType.PanelBakery).GetComponent<PanelBakery>().ReloadPanel();
        if(!isUsing)
            panelBakery.SetCakeToSwap(cakeData.id);
    }

    public void Init(CakeData cakeData)
    {
        this.cakeData = cakeData;
        onIconImg.sprite = cakeData.icon;
        offIconImg.sprite = cakeData.icon;
        cakeNameTxt.text = "Cake " + cakeData.id.ToString();
        cakePointTxt.text = ((cakeData.id + 1) * 5).ToString() + ConstantValue.STR_SPACE + ConstantValue.STR_EXP + PERCAKE;
        currentCake = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCake(cakeData.id);
        InitUsing();
    }

    public void InitUsing()
    {
        isUsing = false;
        if(ProfileManager.Instance.playerData.cakeSaveData.IsOwnedCake(cakeData.id))
        {
            lockingMarkObj.SetActive(false);
            offIconImg.gameObject.SetActive(false);
            onIconImg.gameObject.SetActive(true);
            usingMarkObj.SetActive(ProfileManager.Instance.playerData.cakeSaveData.IsUsingCake(cakeData.id));
            isUsing = ProfileManager.Instance.playerData.cakeSaveData.IsUsingCake(cakeData.id);
            if(currentCake == null) currentCake = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCake(cakeData.id);
            if(currentCake != null)
            {
                cardAmountTxt.text = currentCake.cardAmount.ToString() + ConstantValue.STR_SLASH + currentCake.CardRequire.ToString();
                levelTxt.text = currentCake.level.ToString();
                cardAmountSlider.value = (float)currentCake.cardAmount / (float)currentCake.CardRequire;
            }
        }
        else
        {
            lockingMarkObj.SetActive(true);
            offIconImg.gameObject.SetActive(true);
            onIconImg.gameObject.SetActive(false);
            usingMarkObj.SetActive(false);
            cardAmountTxt.text = ConstantValue.STR_BLANK;
            levelTxt.text = ConstantValue.STR_BLANK;
            cardAmountSlider.value = 0;
        }
    }
}
