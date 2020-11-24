namespace IplServerSide.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingUserNotification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        UserNotificationId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        NotificationObject = c.String(),
                    })
                .PrimaryKey(t => t.UserNotificationId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.Users");
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropTable("dbo.UserNotifications");
        }
    }
}
