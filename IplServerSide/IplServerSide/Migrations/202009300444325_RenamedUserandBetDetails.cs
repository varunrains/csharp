namespace IplServerSide.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedUserandBetDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bets", "IsDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "UserAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Bets", "IsMatchCompleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bets", "IsMatchCompleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "UserAmount", c => c.Int(nullable: false));
            DropColumn("dbo.Bets", "IsDeleted");
        }
    }
}
