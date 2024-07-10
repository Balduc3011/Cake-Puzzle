using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
#if UNITY_ANDROID || UNITY_IOS
using Unity.Notifications.Android;
using UnityEngine;

public class PushNotificationManager : MonoBehaviour {
    private void Start() {
        RegisterChannels();
    }
    public void RegisterChannels() {
        for (int i = 1; i <= 10; i++) {
            string channel = "day-" + i;
            RegisterNotificationChannel(channel, channel);
        }
        ScheduleDailyPush();
    }
    public void ScheduleDailyPush() {
        ClearAllPush();
        Debug.Log("Start schedule");
        List<string> notificationMessages = new List<string>();
        notificationMessages.Add("Let him bake!");
        notificationMessages.Add("Wanna bake me a cake");
        notificationMessages.Add("The cake hasn't baked");
        notificationMessages.Add("Special delivery");
        notificationMessages.Add("Let bake somethings");
        for (int i = 1; i <= 10; i++) {
            string title = "Cake Sort";
            string message = notificationMessages[UnityEngine.Random.Range(0, notificationMessages.Count)];
            long time = i * 24 * 3600 * 1000;
            //time = i * 10 * 1000;
            string channel = "day-" + i;
            ScheduleNotification(channel, title, message, time);
        }
        Debug.Log("End schedule");
    }

    public void ClearAllPush() {
        Debug.Log("Clear All schedule");
#if UNITY_IOS
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
#endif
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllScheduledNotifications();
#endif
    }

    public void AskNotificationPermission() {
#if UNITY_IOS
        UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Sound);
#endif
    }
    public void RegisterNotificationChannel(string channelID, string channelName) {
        var c = new AndroidNotificationChannel() {
            Id = channelID,
            Name = channelName,
            Importance = Importance.High,
            Description = "Generic notifications",
            CanBypassDnd = true,
            CanShowBadge = true,
            EnableLights = true,
            EnableVibration = true,
            LockScreenVisibility = LockScreenVisibility.Private,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }
    public void ScheduleNotification(string channelID, string title, string description, double time) {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = description;
        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";
        notification.FireTime = System.DateTime.Now.AddMilliseconds(time);
        AndroidNotificationCenter.SendNotification(notification, channelID);
    }

}
#endif
#endif