using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IplServerSide.Models
{
    public partial class UserNotification
    {
        public int UserNotificationId { get; set; }
        public int UserId { get; set; }
        public string NotificationObject { get; set; }
        public virtual User User { get; set; }
    }
}