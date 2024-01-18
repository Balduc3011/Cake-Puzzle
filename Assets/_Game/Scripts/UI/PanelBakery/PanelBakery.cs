using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBakery : UIPanel
{
    [SerializeField] UIPanelShowUp uiPanelShowUp;
    [SerializeField] InventoryCake inventoryCakePrefab;
    [SerializeField] List<InventoryCake> inventoryCakeList;
    [SerializeField] Transform inventoryCakeContainer;

    [SerializeField] UsingCake usingCakePrefab;
    [SerializeField] List<UsingCake> usingCakeList;
    [SerializeField] Transform usingCakeContainer;


    public override void Awake()
    {
        panelType = UIPanelType.PanelBakery;
        base.Awake();
        InitCakes();
    }

    void InitCakes()
    {
        for (int i = 0; i < 6; i++)
        {
            UsingCake cake = Instantiate(usingCakePrefab, usingCakeContainer);
            usingCakeList.Add(cake);
        }

        for (int i = 0; i < 10; i++)
        {
            InventoryCake cake = Instantiate(inventoryCakePrefab, inventoryCakeContainer);
            inventoryCakeList.Add(cake);
        }
    }

    public void OnClose()
    {
        uiPanelShowUp.OnClose(UIManager.instance.ClosePanelBakery);
    }
}
