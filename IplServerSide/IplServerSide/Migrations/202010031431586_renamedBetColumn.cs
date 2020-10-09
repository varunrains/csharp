namespace IplServerSide.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedBetColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bets", "NetAmountWon", c => c.Decimal(precision: 6, scale: 2));
            DropColumn("dbo.Bets", "NetAmountOwn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bets", "NetAmountOwn", c => c.Decimal(precision: 6, scale: 2));
            DropColumn("dbo.Bets", "NetAmountWon");
        }
    }
}
