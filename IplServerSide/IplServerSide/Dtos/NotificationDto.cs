using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace IplServerSide.Dtos
{
    public class NotificationChild
    {
        public string title { get; set; }
        public string body { get; set; }
        public string icon { get; set; }
        public string tag { get; set; }
        public bool renotify { get; set; }
        public int[]  vibrate { get; set; }
        public string badge { get; set; }
        public string lang { get; set; }
    }

    public class Notification
    {
        public NotificationChild notification { get; set; }
    }

    public class SubscriptionObject
    {
        public string endpoint { get; set; }
        public Key keys { get; set; }
    }

    public class Key
    {
        public string auth { get; set; }
        public string p256dh { get; set; }
    }
}