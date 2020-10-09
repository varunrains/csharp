namespace IplServerSide.Models
{
    using System;

    public partial class Bet
    {
       
        public int BetId { get; set; }
        public int MatchId { get; set; }
        public int UserId { get; set; }
        public int BettingTeamId { get; set; }
        public int? WinningTeamId { get; set; }
        public decimal? NetAmountWon { get; set; }
        public DateTimeOffset BettingDate { get; set; }
        public int BetAmount { get; set; }
        public bool IsMatchAbandoned { get; set; }
        public virtual Match Match { get; set; }
        public virtual User User { get; set; }
    }
}
