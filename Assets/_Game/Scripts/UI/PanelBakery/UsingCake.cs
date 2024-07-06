using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsingCake : MonoBehaviour
{
    CakeData cakeData;
    [SerializeField] Button button;
    [SerializeField] Image onIconImg;
    [SerializeField] GameObject swapIcon;
    PanelBakery panelBakery;
    void Start()
    {
        button.onClick.AddListener(OnCakeClick);
        panelBakery = UIManager.instance.GetPanel(UIPanelType.PanelBakery).GetComponent<PanelBakery>();
    }

    void OnCakeClick()
    {
        SwapCake();
        panelBakery.ReloadPanel();
        return;
        //transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        //{
        //    gameObject.SetActive(false);
        //    ProfileManager.Instance.playerData.cakeSaveData.RemoveUsingCake(cakeData.id);
        //    panelBakery.RemoveUsingCake(this);
        //    panelBakery.ReloadPanel();
        //});
    }

    public void SwapCake()
    {
        int newCakeId = panelBakery.toSwapCake;
        if(newCakeId != -1)
        {
            ProfileManager.Instance.playerData.cakeSaveData.SwapCake(cakeData.id, newCakeId);
            Init(ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(newCakeId));
            panelBakery.OnCakeSwaped();
        }
    }

    public void Init(CakeData cakeData)
    {
        this.cakeData = cakeData;
        if (cakeData != null)
        {
            Debug.Log("111");
            int levelPref = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(cakeData.id);
            if (levelPref > 2) levelPref = 2;
            onIconImg.sprite = cakeData.icons[levelPref - 1];
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 0.25f).SetEase(Ease.OutBack);
        }
    }

    public void SetSwapable(bool active)
    {
        button.interactable = active;
        swapIcon.SetActive(active);
    }
}
