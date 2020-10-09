namespace IplServerSide.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedUnusedColumns : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bets", "IsDeleted");
            DropColumn("dbo.Users", "IsAllowedToBet");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "IsAllowedToBet", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bets", "IsDeleted", c => c.Boolean(nullable: false));
        }
    }
}
