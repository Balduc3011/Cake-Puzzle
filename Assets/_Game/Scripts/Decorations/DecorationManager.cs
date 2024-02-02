using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    PanelDecorations panelDecorations;
    public DecorationComponent decorationComponent;

    public void StartCamera(bool start)
    {
        decorationComponent.StartCamera(start);
    }

    public bool IsOwned(DecorationType type, int id)
    {
        return ProfileManager.Instance.playerData.decorationSave.IsOwned(type, id);
    }

    public bool IsInUse(DecorationType type, int id)
    {
        return ProfileManager.Instance.playerData.decorationSave.IsInUse(type, id);
    }

    public void UseDecor(DecorationType type, int id)
    {
        ProfileManager.Instance.playerData.decorationSave.UseDecor(type, id);
        if (panelDecorations == null)
            panelDecorations = UIManager.instance.GetPanel(UIPanelType.PanelDecorations).GetComponent<PanelDecorations>();
        panelDecorations.InitDecorationData(type, true);
    }

    public void BuyDecor(DecorationType type, int id)
    {
        float price = ProfileManager.Instance.dataConfig.decorationDataConfig.GetDecorPrice(type, id);
        if(ProfileManager.Instance.playerData.playerResourseSave.IsHasEnoughMoney(price))
        {
            ProfileManager.Instance.playerData.playerResourseSave.ConsumeMoney(price);
            ProfileManager.Instance.playerData.decorationSave.BuyDecor(type, id);
            UseDecor(type, id);
        }
        if(panelDecorations == null) 
            panelDecorations = UIManager.instance.GetPanel(UIPanelType.PanelDecorations).GetComponent<PanelDecorations>();
        panelDecorations.InitDecorationData(type, true);
    }

    public void ShowComponent(DecorationType decorationType)
    {
        decorationComponent.ShowComponent(decorationType);
    }
}
