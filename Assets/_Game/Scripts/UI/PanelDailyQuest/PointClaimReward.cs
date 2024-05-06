using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIAnimation;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class PointClaimReward : MonoBehaviour
{
    [SerializeField] RectTransform myRect;
    [SerializeField] Image imgIcon;
    [SerializeField] TextMeshProUGUI txtPointClaim;
    [SerializeField] TextMeshProUGUI txtReward;
    [SerializeField] Vector3 vetorOffSet;
    [SerializeField] Animator anim;
    [SerializeField] Button btnClaim;
    [SerializeField] GameObject objCheck;

    float reward;
    ItemType itemType;
    private void Awake()
    {
        btnClaim.onClick.AddListener(ClaimReward);
    }
    public void InitData(Vector2 vectorScale, Vector3 pointTransform, int reward, int pointClaim, ItemType itemType, bool canClaim, bool earned)
    {
        this.reward = reward;
        this.itemType = itemType;
        transform.position = pointTransform;
        myRect.sizeDelta = vectorScale;
        imgIcon.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetItemSprite(itemType);
        txtPointClaim.text = pointClaim.ToString();
        txtReward.text = "+" + reward;
        SetUpMode(canClaim);
        objCheck.gameObject.SetActive(earned);
    }

    public void SetUpMode(bool canClaim) {
        if (canClaim)
            ActiveMode();
        else
            DisAbleMode();
    }

    public void ActiveMode() {
        anim.SetBool("Move", true);
        btnClaim.interactable = true;
    }

    public void DisAbleMode() {
        anim.SetBool("Move", false);
        btnClaim.interactable = false;
    }

    void ClaimReward() {
        //GameManager.Instance.ClaimReward(itemType, reward);
        DisAbleMode();
        objCheck.gameObject.SetActive(true);   
    }
}
