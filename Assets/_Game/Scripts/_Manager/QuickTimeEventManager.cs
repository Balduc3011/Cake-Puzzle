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

    [SerializeField] int currentCakeID = -1;

    public bool isFail;
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.playing ||
            ProfileManager.Instance.playerData.playerResourseSave.currentLevel <= 4)
        {
            timeGamePlay = 0;
            return;
        }

        if (!ProfileManager.Instance.playerData.playerResourseSave.isFirstTimeLevelUpFive)
        {
            timeGamePlay = 175f;
            ProfileManager.Instance.playerData.playerResourseSave.LevelUpFiveFirsTime();
            return;
        }

        if (!onQuickTimeEvent  && !isFail)
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
                    currentProgress = 0;
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
            else EndQuickTimeEvent(true);
        }

    }

    public void InitMission() {
        cakeNeedDone = 3;
        timeTotal = 5f * 60f;
        timeMissionRemain = timeTotal;
        currentCakeID = ProfileManager.Instance.playerData.cakeSaveData.GetCakeIDForMission();
    }

    public float GetTimeQuickTimeEvent() {
        return timeTotal;
    }

    public int GetTotalCakeNeedDone() {

        return cakeNeedDone;
    }

    public void EndQuickTimeEvent(bool callbymissionfail) {

        onQuickTimeEvent = false;
        UIManager.instance.panelTotal.OutTimeEvent();
        if (callbymissionfail)
        {
            isFail = true;
            UIManager.instance.ShowPanelLevelComplete(false);
        }
    }

    public void AddProgess(int cakeID, Transform pointCake) {
        if (cakeID != currentCakeID || !onQuickTimeEvent) return;

        currentProgress++;
        UIManager.instance.panelTotal.UpdateQuickTimeEvent(currentProgress, pointCake);
        if (currentProgress >= cakeNeedDone)
        {
            EndQuickTimeEvent(false);
            GameManager.Instance.RandonReward();
            UIManager.instance.ShowPanelSelectReward();
        }
    }

    public int GetCurrentCakeID() { return currentCakeID; }
}
