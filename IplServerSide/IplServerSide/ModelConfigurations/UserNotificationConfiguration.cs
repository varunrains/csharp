using IplServerSide.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace IplServerSide.ModelConfigurations
{
    public class UserNotificationConfiguration: EntityTypeConfiguration<UserNotification>
    {
        public UserNotificationConfiguration()
        {
            ToTable("UserNotifications");
            HasKey(x => x.UserNotificationId);
            Property(x => x.NotificationObject).IsOptional();

            HasRequired(x => x.User)
                .WithMany(y => y.UserNotifications);
        }
    }
}