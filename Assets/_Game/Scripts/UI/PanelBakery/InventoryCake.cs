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
    [SerializeField] GameObject upgradeBtnObj;
    [SerializeField] ParticleImage upgradeParticle;
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
                upgradeBtnObj.SetActive(currentCake.IsAbleToUpgrade());
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

    void OnCakeClick()
    {
        //if (onIconImg.gameObject.activeSelf)
        //    onIconImg.transform.DOScale(1.2f, 0.25f).OnComplete(() =>
        //    {
        //        onIconImg.transform.DOScale(1, 0.05f);
        //    });
        //if (offIconImg.gameObject.activeSelf)
        //    offIconImg.transform.DOScale(1.2f, 0.25f).OnComplete(() =>
        //    {
        //        offIconImg.transform.DOScale(1, 0.05f);
        //    });
        transform.DOScale(0.9f, 0.05f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                transform.DOScale(1f, 0.05f).SetEase(Ease.InOutQuad);
            });

        if (currentCake != null)
        {
            if(currentCake.IsAbleToUpgrade())
            {
                if(GameManager.Instance.IsHasNoAds())
                    OnUpgradeCake();
                else
                    AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.UpgradeCake.ToString(), OnUpgradeCake);
                return;
            }
        }
        if (!isUsing)
            panelBakery.SetCakeToSwap(cakeData.id);
    }

    void OnUpgradeCake()
    {
        if(currentCake != null)
        {
            ProfileManager.Instance.playerData.cakeSaveData.OnUpgradeCard(currentCake);
            upgradeParticle.Play();
            InitUsing();
        }
    }
}
