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
    }

    private void OnEnable()
    {
        if(inited)
            ReloadPanel(true);
        OnCakeSwaped();
    }

    void InitCakes()
    {
        List<int> usingCakeIndex = ProfileManager.Instance.playerData.cakeSaveData.cakeIDUsing;
        for (int i = 0; i < usingCakeIndex.Count; i++)
        {
            UsingCake cake = GetUsingCake();
            cake.Init(ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(usingCakeIndex[i]));
        }

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
        if(reloadUsing)
        {
            List<int> usingCakeIndex = ProfileManager.Instance.playerData.cakeSaveData.cakeIDUsing;
            for (int i = 0; i < usingCakeIndex.Count; i++)
            {
                if (i < usingCakeList.Count)
                {
                    if (!usingCakeList[i].gameObject.activeSelf)
                    {
                        usingCakeList[i].Init(ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(usingCakeIndex[i]));
                    }
                }
                else
                {
                    UsingCake cake = GetUsingCake();
                    cake.Init(ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeData(usingCakeIndex[i]));
                }
            }
        }
    }

    public UsingCake GetUsingCake()
    {
        for (int i = 0;i < usingCakeList.Count;i++)
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
}
