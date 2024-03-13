using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAPButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] PackageId packageId;

    [SerializeField] TextMeshProUGUI valueTxt;
    void Start()
    {
        button.onClick.AddListener(IAPButtonOnClick);
        UpdateValue();
        EventManager.AddListener(EventName.ChangeCoin.ToString(), UpdateValue);    
    }

    void IAPButtonOnClick()
    {
        UIManager.instance.ShowPanelQuickIAP(packageId);
    }

    void UpdateValue()
    {
        switch (packageId)
        {
            case PackageId.None:
                break;
            case PackageId.Piggy:
                if(valueTxt != null)
                    valueTxt.text = ProfileManager.Instance.playerData.playerResourseSave.piggySave.ToString();
                break;
            case PackageId.Pack1:
                break;
            default:
                break;
        }
    }
}
