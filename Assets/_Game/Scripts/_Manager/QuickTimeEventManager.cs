using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeEventManager : MonoBehaviour
{
    bool onQuickTimeEvent;
    float timeGamePlay;
    [SerializeField] float timeGamePlaySetting;
    [SerializeField] int cakeNeedDone;
    [SerializeField] float timeQuickEvent;

    [Range(0, 15)]
    [SerializeField] int minCakeNeedDone;
    [Range(15, 40)]
    [SerializeField] int maxCakeNeedDone = 15;
    // Update is called once per frame
    void Update()
    {
        if (!onQuickTimeEvent)
        {
            if (timeGamePlay < timeGamePlaySetting)
            {
                timeGamePlay += Time.deltaTime;
            }
            else
            {
                UIManager.instance.ShowPanelQuickTimeEvent();
                onQuickTimeEvent = true;
                timeGamePlay = 0;
            }
        }
       
    }

    public float GetTimeQuickTimeEvent() { return timeQuickEvent = cakeNeedDone * 5f; }
    public int GetTotalCakeNeedDone() { return cakeNeedDone = Random.Range(minCakeNeedDone, maxCakeNeedDone); }
}
