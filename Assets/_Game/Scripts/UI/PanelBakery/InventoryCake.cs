using AssetKits.ParticleImage;
using DG.Tweening;
using SDK;
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
    [SerializeField] GameObject levelBar;
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] GameObject upgradeNotify;
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

    public void Init(CakeData cakeData)
    {
        this.cakeData = cakeData;
        onIconImg.sprite = cakeData.icon;
        offIconImg.sprite = cakeData.icon;
        cakeNameTxt.text = "Cake " + cakeData.id.ToString();
        cakePointTxt.text = ((cakeData.id + 1) * 5).ToString() + ConstantValue.STR_SPACE + ConstantValue.STR_EXP + PERCAKE;
        InitUsing();
    }

    public void InitUsing()
    {
        isUsing = false;
        currentCake = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCake(cakeData.id);
        if (ProfileManager.Instance.playerData.cakeSaveData.IsOwnedCake(cakeData.id))
        {
            levelBar.SetActive(true);
            levelTxt.text = currentCake.level.ToString();
            lockingMarkObj.SetActive(false);
            offIconImg.gameObject.SetActive(false);
            onIconImg.gameObject.SetActive(true);
            usingMarkObj.SetActive(ProfileManager.Instance.playerData.cakeSaveData.IsUsingCake(cakeData.id));
            isUsing = ProfileManager.Instance.playerData.cakeSaveData.IsUsingCake(cakeData.id);
            if(currentCake == null) currentCake = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCake(cakeData.id);
            upgradeNotify.SetActive(currentCake.IsAbleToUpgrade());
        }
        else
        {
            levelBar.SetActive(false);
            levelTxt.text = ConstantValue.STR_BLANK;
            lockingMarkObj.SetActive(true);
            offIconImg.gameObject.SetActive(true);
            onIconImg.gameObject.SetActive(false);
            usingMarkObj.SetActive(false);
            upgradeNotify.SetActive(false);
        }
    }

    void OnCakeClick()
    {
        transform.DOScale(0.9f, 0.05f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                transform.DOScale(1f, 0.05f).SetEase(Ease.InOutQuad);
        });
        panelBakery.ShowCakeInfo(cakeData, this);
    }
}
