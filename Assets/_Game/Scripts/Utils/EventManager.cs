using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UnityEventObject : UnityEvent<object>
{

}

public enum EventName
{
    None = 0,
    TurnOffAllADS = 1,
    ChangeCoin = 2,
    ChangeExp = 3,
    ChangeLevel = 4,
    ChangeFloorDecor = 5,
    ChangePlateDecor = 6,
    ChangeTableDecor = 7,
    UsingFillUp = 8,
    UsingFillUpDone = 9,
    UpdateCakeOnPlate = 10,
    ChangeStarDailyQuest = 11,
    AddCakeCard = 12,
    UsingHammer = 13,
    UsingHammerDone = 14,
    CheckClearCake = 15,
    AddItem = 16,
    OnUsingRevive = 17,
    OnUsingReviveDone = 18,
    UpgradeCakeCard = 19,
}

public class EventManager : Singleton<EventManager>
{
    private Dictionary<string, UnityEvent> eventDic = new Dictionary<string, UnityEvent>();

    private Dictionary<string, UnityEventObject> eventDictionary = new Dictionary<string, UnityEventObject>();
    private List<UnityEvent> eventStack = new List<UnityEvent>();

    protected override void Awake() {
        if(_instance == null) {
            base.Awake();
        } else {
            DestroyImmediate(gameObject);
        }
    }
    public static void AddListener(string name, UnityAction action) {
        UnityEvent thisEvent = null;
        if (Instance.eventDic.TryGetValue(name, out thisEvent)) {
            thisEvent.AddListener(action);
        } else {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(action);
            Instance.eventDic.Add(name, thisEvent);
        }
    }

    public static void RemoveListener(string name, UnityAction action)
    {
        if (Instance == null) return;
        UnityEvent thisEvent = null;
        if (Instance.eventDic.TryGetValue(name, out thisEvent))
        {
            thisEvent.RemoveListener(action);
        }
    }

    public static void TriggerEvent(string name) {
        UnityEvent thisEvent = null;
        if (Instance) {
            if (Instance.eventDic.TryGetValue(name, out thisEvent)) {
                thisEvent.Invoke();
            }
        }
    }

    public static void AddListener(string eventName, UnityAction<object> listener) {
        UnityEventObject thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        } else {
            thisEvent = new UnityEventObject();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void RemoveListener(string eventName, UnityAction<object> listener) {
        UnityEventObject thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, object param = null) {
        UnityEventObject thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke(param);
        }
    }
    public static void AddEventNextFrame(UnityAction listener) {
        UnityEvent thisEvent = new UnityEvent();
        thisEvent.AddListener(listener);
        Instance.eventStack.Add(thisEvent);
    }
    private void Update() {
        while (Instance.eventStack.Count > 0) {
            UnityEvent thisEvent = Instance.eventStack[0];
            thisEvent.Invoke();
            Instance.eventStack.RemoveAt(0);
        }
    }
}