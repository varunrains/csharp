namespace IplServerSide.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bets",
                c => new
                    {
                        BetId = c.Int(nullable: false, identity: true),
                        MatchId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        BettingTeamId = c.Int(nullable: false),
                        WinningTeamId = c.Int(),
                        NetAmountOwn = c.Decimal(precision: 6, scale: 2),
                        BettingDate = c.DateTimeOffset(nullable: false, precision: 7),
                        BetAmount = c.Int(nullable: false),
                        IsMatchAbandoned = c.Boolean(nullable: false),
                        IsMatchCompleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BetId)
                .ForeignKey("dbo.Matches", t => t.MatchId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => new { t.MatchId, t.UserId }, unique: true, name: "IX_MatchIdAndUserId");
            
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        MatchId = c.Int(nullable: false, identity: true),
                        MatchDateTime = c.DateTimeOffset(nullable: false, precision: 7),
                        TeamIdA = c.Int(nullable: false),
                        TeamIdB = c.Int(nullable: false),
                        Place = c.String(nullable: false, maxLength: 256),
                        Result = c.Int(),
                    })
                .PrimaryKey(t => t.MatchId)
                .ForeignKey("dbo.Teams", t => t.TeamIdA, cascadeDelete: false)
                .ForeignKey("dbo.Teams", t => t.TeamIdB, cascadeDelete: false)
                .Index(t => t.TeamIdA)
                .Index(t => t.TeamIdB);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(nullable: false, maxLength: 256),
                        TeamShortName = c.String(nullable: false, maxLength: 8),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 24),
                        DisplayName = c.String(),
                        UserGroup = c.String(),
                        PassKey = c.String(nullable: false, maxLength: 24),
                        UserAmount = c.Int(nullable: false),
                        IsAllowedToBet = c.Boolean(nullable: false),
                        UserRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bets", "UserId", "dbo.Users");
            DropForeignKey("dbo.Bets", "MatchId", "dbo.Matches");
            DropForeignKey("dbo.Matches", "TeamIdB", "dbo.Teams");
            DropForeignKey("dbo.Matches", "TeamIdA", "dbo.Teams");
            DropIndex("dbo.Matches", new[] { "TeamIdB" });
            DropIndex("dbo.Matches", new[] { "TeamIdA" });
            DropIndex("dbo.Bets", "IX_MatchIdAndUserId");
            DropTable("dbo.Users");
            DropTable("dbo.Teams");
            DropTable("dbo.Matches");
            DropTable("dbo.Bets");
        }
    }
}
