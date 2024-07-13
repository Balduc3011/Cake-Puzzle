using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuickTimeEventManager : MonoBehaviour
{
    public bool onQuickTimeEvent;
    [SerializeField] float timeGamePlay;
    [SerializeField] float timeGamePlaySetting;
    [SerializeField] int cakeNeedDone;
    [SerializeField] int currentProgress;
    [SerializeField] float timeTotal;
    [SerializeField] float timeMissionRemain = 0;

    [Range(0, 15)]
    [SerializeField] int minCakeNeedDone;
    [Range(15, 40)]
    [SerializeField] int maxCakeNeedDone = 15;

    [SerializeField] int currentCakeID = -1;

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
                    timeGamePlay = 0;
                    timeMissionRemain = 1000;
                    UIManager.instance.ShowPanelQuickTimeEvent();
                    onQuickTimeEvent = true;
                }
            }
        }

        if (onQuickTimeEvent) {
            if (timeMissionRemain > 0)
            {
                timeMissionRemain -= Time.deltaTime;
                UIManager.instance.panelTotal.UpdateTime(timeMissionRemain);
            }
            else EndQuickTimeEvent();
        }

    }

    public void InitMission() {
        cakeNeedDone = Random.Range(minCakeNeedDone, maxCakeNeedDone);
        timeTotal = cakeNeedDone * 15f;
        timeMissionRemain = timeTotal;
        currentCakeID = ProfileManager.Instance.playerData.cakeSaveData.GetCakeIDForMission();
    }

    public float GetTimeQuickTimeEvent() {
        return timeTotal;
    }

    public int GetTotalCakeNeedDone() {

        return cakeNeedDone;
    }

    public void EndQuickTimeEvent() {
        onQuickTimeEvent = false;
        UIManager.instance.panelTotal.OutTimeEvent();
    }

    public void AddProgess(int cakeID, Transform pointCake) {
        if (cakeID != currentCakeID || !onQuickTimeEvent) return;

        currentProgress++;
        UIManager.instance.panelTotal.UpdateQuickTimeEvent(currentProgress, pointCake);
        if (currentProgress >= cakeNeedDone)
        {
            EndQuickTimeEvent();
            GameManager.Instance.RandonReward();
            UIManager.instance.ShowPanelSelectReward();
        }
    }

    public int GetCurrentCakeID() { return currentCakeID; }
}
