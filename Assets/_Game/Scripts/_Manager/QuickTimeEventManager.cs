using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeEventManager : MonoBehaviour
{
    public bool onQuickTimeEvent;
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
        if (!GameManager.Instance.playing &&
            ProfileManager.Instance.playerData.playerResourseSave.currentLevel <= 4)
        {
            timeGamePlay = 0;
            return;
        }
        if (!onQuickTimeEvent)
        {
            if (timeGamePlay < timeGamePlaySetting)
            {
                timeGamePlay += Time.deltaTime;
            }
            else
            {
                if (UIManager.instance.isHasPopupOnScene)
                    timeGamePlay -= 10f;
                else
                {
                    UIManager.instance.ShowPanelQuickTimeEvent();
                    onQuickTimeEvent = true;
                    timeGamePlay = 0;
                }
            }
        }
       
    }

    public float GetTimeQuickTimeEvent() { return timeQuickEvent = cakeNeedDone * 15f; }
    public int GetTotalCakeNeedDone() { return cakeNeedDone = Random.Range(minCakeNeedDone, maxCakeNeedDone); }
    public void EndQuickTimeEvent() { onQuickTimeEvent = false; }
    public void AddProgess() {
        if (onQuickTimeEvent) UIManager.instance.panelTotal.UpdateQuickTimeEvent();
    }
}
