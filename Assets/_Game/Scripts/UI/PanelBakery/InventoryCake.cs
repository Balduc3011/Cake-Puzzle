using AssetKits.ParticleImage;
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
    [SerializeField] Button button;
    [SerializeField] Image onIconImg;
    [SerializeField] Image offIconImg;
    [SerializeField] GameObject usingMarkObj;
    [SerializeField] GameObject lockingMarkObj;
    PanelBakery panelBakery;
    bool isUsing;
    [SerializeField] GameObject levelBar;
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] GameObject upgradeNotify;
    [SerializeField] Slider cardAmountSlider;
    [SerializeField] TextMeshProUGUI cardAmountTxt;
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
        int levelPref = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(cakeData.id);
        if (levelPref > 2) levelPref = 2;
        onIconImg.sprite = cakeData.icons[levelPref - 1];
        offIconImg.sprite = cakeData.icons[levelPref - 1];
        InitUsing();
    }

    public void InitUsing()
    {
        isUsing = false;
        currentCake = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCake(cakeData.id);
        int levelPref = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(cakeData.id);
        if(levelPref > 2) levelPref = 2;
        onIconImg.sprite = cakeData.icons[levelPref - 1];
        offIconImg.sprite = cakeData.icons[levelPref - 1];

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
            float upgradePrice = upgradePrice = ConstantValue.VAL_CAKEUPGRADE_COIN * currentCake.level;
            upgradeNotify.SetActive(currentCake.IsAbleToUpgrade() &&
                ProfileManager.Instance.playerData.playerResourseSave.IsHasEnoughMoney(upgradePrice));
            cardAmountSlider.value = (float)currentCake.cardAmount / (float)currentCake.CardRequire;
            cardAmountSlider.gameObject.SetActive(true);
            cardAmountTxt.text = currentCake.cardAmount.ToString() + ConstantValue.STR_SLASH + currentCake.CardRequire.ToString();
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
            cardAmountSlider.gameObject.SetActive(false);
        }
    }

    void OnCakeClick()
    {
        transform.DOScale(0.9f, 0.05f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                transform.DOScale(1f, 0.05f).SetEase(Ease.InOutQuad);
        });
        if(ProfileManager.Instance.playerData.cakeSaveData.IsOwnedCake(cakeData.id)) {
            panelBakery.ShowCakeInfo(cakeData, this);
        }
    }
}
