using DG.Tweening;
using SDK;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelBakery : UIPanel
{
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    [SerializeField] GameObject lobbyTitle;
    [SerializeField] GameObject playingTitle;
    [SerializeField] Button closeBtn;
    [SerializeField] InventoryCake inventoryCakePrefab;
    [SerializeField] List<InventoryCake> inventoryCakeList;
    [SerializeField] Transform inventoryCakeContainer;

    [SerializeField] UsingCake usingCakePrefab;
    [SerializeField] List<UsingCake> usingCakeList;
    [SerializeField] Transform usingCakeContainer;
    public int toSwapCake;

    bool inited = false;
    public override void Awake()
    {
        panelType = UIPanelType.PanelBakery;
        base.Awake();
    }

    private void Start()
    {
        InitCakes();
        SetUpPopupBtn();
        closeBtn.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        if (inited)
            ReloadPanel(true);
        OnCakeSwaped();
        cakeInfoPopup.SetActive(false);
        lobbyTitle.SetActive(!GameManager.Instance.playing);
        playingTitle.SetActive(GameManager.Instance.playing);

    }

    void InitCakes()
    {
        //List<int> usingCakeIndex = ProfileManager.Instance.playerData.cakeSaveData.cakeIDUsing;
        //for (int i = 0; i < usingCakeIndex.Count; i++)
        //{
        //    UsingCake cake = GetUsingCake();
        //    cake.Init(ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(usingCakeIndex[i]));
        //}

        List<CakeData> cakeDatas = ProfileManager.Instance.dataConfig.cakeDataConfig.cakeDatas;
        for (int i = 0; i < cakeDatas.Count; i++)
        {
            InventoryCake cake = Instantiate(inventoryCakePrefab, inventoryCakeContainer);
            inventoryCakeList.Add(cake);
            cake.Init(cakeDatas[i]);
        }
        inited = true;
    }

    public void RemoveUsingCake(UsingCake cake)
    {
        usingCakeList.Remove(cake);
        usingCakeList.Add(cake);
        cake.transform.SetAsLastSibling();
    }

    public void ReloadPanel(bool reloadUsing = false)
    {
        for (int i = 0; i < inventoryCakeList.Count; i++)
        {
            inventoryCakeList[i].InitUsing();
        }
        //if (reloadUsing)
        //{
        //    List<int> usingCakeIndex = ProfileManager.Instance.playerData.cakeSaveData.cakeIDUsing;
        //    for (int i = 0; i < usingCakeIndex.Count; i++)
        //    {
        //        if (i < usingCakeList.Count)
        //        {
        //            if (!usingCakeList[i].gameObject.activeSelf)
        //            {
        //                usingCakeList[i].Init(ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(usingCakeIndex[i]));
        //            }
        //        }
        //        else
        //        {
        //            UsingCake cake = GetUsingCake();
        //            cake.Init(ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(usingCakeIndex[i]));
        //        }
        //    }
        //}
    }

    public UsingCake GetUsingCake()
    {
        for (int i = 0; i < usingCakeList.Count; i++)
        {
            if (!usingCakeList[i].gameObject.activeSelf)
                return usingCakeList[i];
        }
        UsingCake cake = Instantiate(usingCakePrefab, usingCakeContainer);
        usingCakeList.Add(cake);
        return cake;
    }

    public void OnClose()
    {
        uiPanelShowUp.OnClose(UIManager.instance.ClosePanelBakery);
    }

    public void SetCakeToSwap(int toSwapCake)
    {
        this.toSwapCake = toSwapCake;
        for (int i = 0; i < usingCakeList.Count; i++)
        {
            usingCakeList[i].SetSwapable(true);
        }
    }

    public void OnCakeSwaped()
    {
        toSwapCake = -1;
        for (int i = 0; i < usingCakeList.Count; i++)
        {
            usingCakeList[i].SetSwapable(false);
        }
    }

    void ClosePanel()
    {
        UIManager.instance.ClosePanelBakery();
    }

    [SerializeField] GameObject cakeInfoPopup;
    [SerializeField] UIPanelAnimOpenAndClose popUpAnim;
    [SerializeField] Button popupCloseBtn1;
    [SerializeField] Button popupCloseBtn2;
    [SerializeField] Button upgradeCakeBtn;
    [SerializeField] GameObject upradeAbleParticle;
    [SerializeField] Slider cardAmountSlider;
    [SerializeField] TextMeshProUGUI cardAmountTxt;
    [SerializeField] TextMeshProUGUI levelTxt;

    [SerializeField] TextMeshProUGUI expTxt;
    [SerializeField] TextMeshProUGUI trophyTxt;
    [SerializeField] TextMeshProUGUI coinTxt;
    [SerializeField] List<Transform> statsBar;

    [SerializeField] Button useCakeBtn;
    [SerializeField] GameObject usingCakeBtnObj;
    [SerializeField] GameObject noCakeBtnObj;

    CakeData popupCake;
    OwnedCake currentCake;
    InventoryCake inventoryCake;
    public void ShowCakeInfo(CakeData cakeData, InventoryCake inventoryCake)
    {
        this.inventoryCake = inventoryCake;
        popupCake = cakeData;
        cakeInfoPopup.SetActive(true);
        LoadPopup();
    }

    void LoadPopup()
    {
        PopupAnim();
        GameManager.Instance.cakeManager.cakeShowComponent.ShowSelectetCake(popupCake.id);
        currentCake = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCake(popupCake.id);
        if (ProfileManager.Instance.playerData.cakeSaveData.IsOwnedCake(popupCake.id))
        {
            levelTxt.text = "Level " + currentCake.level.ToString();
            cardAmountSlider.value = (float)currentCake.cardAmount / (float)currentCake.CardRequire;
            cardAmountTxt.text = currentCake.cardAmount.ToString() + ConstantValue.STR_SLASH + currentCake.CardRequire.ToString();
            upgradeCakeBtn.interactable = currentCake.IsAbleToUpgrade();
            upradeAbleParticle.SetActive(upgradeCakeBtn.interactable);
            expTxt.text = (GameManager.Instance.GetDefaultCakeProfit(popupCake.id, currentCake.level)).ToString() + " exp";
            trophyTxt.text = (GameManager.Instance.GetDefaultCakeProfit(popupCake.id, currentCake.level)).ToString() + " trophy";
            coinTxt.text = (GameManager.Instance.GetDefaultCakeProfit(popupCake.id, currentCake.level)).ToString() + " coin";
            useCakeBtn.interactable = (!ProfileManager.Instance.playerData.cakeSaveData.IsUsingCake(popupCake.id));
            usingCakeBtnObj.SetActive(ProfileManager.Instance.playerData.cakeSaveData.IsUsingCake(popupCake.id));
            noCakeBtnObj.SetActive(false);
        }
        else
        {
            cardAmountTxt.text = ConstantValue.STR_BLANK;
            levelTxt.text = "Level 0";
            cardAmountSlider.value = 0;
            upgradeCakeBtn.interactable = false;
            upradeAbleParticle.SetActive(upgradeCakeBtn.interactable);
            expTxt.text = "0";
            trophyTxt.text = "0";
            coinTxt.text = "0";
            useCakeBtn.interactable = false;
            noCakeBtnObj.SetActive(true);
        } 
    }

    void PopupAnim()
    {
        levelTxt.transform.DOScale(1, 0.15f).From(0f).SetEase(Ease.OutBack).SetDelay(0.25f + 0.1f);
        cardAmountSlider.transform.DOScale(1, 0.15f).From(0f).SetEase(Ease.OutBack).SetDelay(0.25f + 0.2f);
        upgradeCakeBtn.transform.DOScale(1, 0.15f).From(0f).SetEase(Ease.OutBack).SetDelay(0.25f + 0.3f);
        statsBar[0].DOScale(1, 0.15f).From(0f).SetEase(Ease.OutBack).SetDelay(0.25f + 0.4f);
        statsBar[1].DOScale(1, 0.15f).From(0f).SetEase(Ease.OutBack).SetDelay(0.25f + 0.45f);
        statsBar[2].DOScale(1, 0.15f).From(0f).SetEase(Ease.OutBack).SetDelay(0.25f + 0.5f);
        useCakeBtn.transform.DOScale(1, 0.15f).From(0f).SetEase(Ease.OutBack).SetDelay(0.25f + 0.55f);
    }

    void SetUpPopupBtn()
    {
        popupCloseBtn1.onClick.AddListener(ClosePopup);
        popupCloseBtn2.onClick.AddListener(ClosePopup);
        useCakeBtn.onClick.AddListener(UseCake);
        upgradeCakeBtn.onClick.AddListener(UpgradeCake);
    }

    void ClosePopup()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        popUpAnim.OnClose(UnActivePopup);
    }
    void UnActivePopup()
    {
        cakeInfoPopup.SetActive(false);
    }

    void UseCake()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        SetCakeToSwap(popupCake.id);
        ClosePopup();
    }

    void UpgradeCake()
    {
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_UIButton);
        if (currentCake != null)
        {
            if (currentCake.IsAbleToUpgrade())
            {
                if (GameManager.Instance.IsHasNoAds())
                {
                    OnUpgradeCake();
                    LoadPopup();
                    inventoryCake?.InitUsing();
                }
                else
                {
                    AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.UpgradeCake.ToString(), OnUpgradeCake);
                    LoadPopup();
                    inventoryCake?.InitUsing();
                }
                return;
            }
        }
        
    }

    void OnUpgradeCake()
    {
        if (currentCake != null)
        {
            ProfileManager.Instance.playerData.cakeSaveData.OnUpgradeCard(currentCake);
            //upgradeParticle.Play();
            EventManager.TriggerEvent(EventName.AddCakeCard.ToString());
            ReloadPanel(true);
        }
        
    }
}
