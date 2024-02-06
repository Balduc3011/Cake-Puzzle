using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    PanelDecorations panelDecorations;
    public DecorationComponent decorationComponent;

    public Dictionary<int, GameObject> floorObjectDecoration = new Dictionary<int, GameObject>();
    public Transform spawnContainer;

    private void Start()
    {
        UpdateFloorDecor();
        EventManager.AddListener(EventName.ChangeFloorDecor.ToString(), UpdateFloorDecor);
    }

    public void StartCamera(bool start)
    {
        decorationComponent.StartCamera(start);
        GameManager.Instance.cameraManager.mainCamera.gameObject.SetActive(!start);
        GameManager.Instance.cameraManager.ShowRoomCamera(!start);
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
        decorationComponent.InitDecoration(type);
        switch (type)
        {
            case DecorationType.None:
                break;
            case DecorationType.Table:
                EventManager.TriggerEvent(EventName.ChangeTableDecor.ToString());
                break;
            case DecorationType.Plate:
                EventManager.TriggerEvent(EventName.ChangePlateDecor.ToString());
                break;
            case DecorationType.Floor:
                EventManager.TriggerEvent(EventName.ChangeFloorDecor.ToString());
                break;
            case DecorationType.Effect:
                break;
            default:
                break;
        }
        
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

    public void UpdateFloorDecor()
    {
        int allPlateCount = ProfileManager.Instance.dataConfig.decorationDataConfig.GetDecorationDataList(DecorationType.Floor).decorationDatas.Count;
        int currentId = ProfileManager.Instance.playerData.decorationSave.GetUsingDecor(DecorationType.Floor);
        for (int i = 0; i < allPlateCount; i++)
        {
            if (floorObjectDecoration.ContainsKey(i))
            {
                if (i != currentId)
                {
                    floorObjectDecoration[i].SetActive(false);
                }
                else
                {
                    floorObjectDecoration[i].SetActive(true);
                }
            }
        }
        if (!floorObjectDecoration.ContainsKey(currentId))
        {
            GameObject newDecor = Instantiate(Resources.Load("Decoration/Floor/" + currentId.ToString()) as GameObject, spawnContainer);
            floorObjectDecoration.Add(currentId, newDecor);
        }
    }
}
