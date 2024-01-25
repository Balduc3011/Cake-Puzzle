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
    PanelBakery panelBakery;
    void Start()
    {
        button.onClick.AddListener(OnCakeClick);
        panelBakery = UIManager.instance.GetPanel(UIPanelType.PanelBakery).GetComponent<PanelBakery>();
    }

    void OnCakeClick()
    {
        transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            ProfileManager.Instance.playerData.cakeSave.RemoveUsingCake(cakeData.id);
            panelBakery.RemoveUsingCake(this);
            panelBakery.ReloadPanel();
        });
    }

    public void Init(CakeData cakeData)
    {
        this.cakeData = cakeData;
        if (cakeData != null)
        {
            onIconImg.sprite = cakeData.icon;
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 0.25f).SetEase(Ease.OutBack);
        }
    }
}
