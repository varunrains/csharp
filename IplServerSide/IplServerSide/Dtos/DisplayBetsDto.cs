using System;

namespace IplServerSide.Dtos
{
    public class DisplayBetsDto :BetDto
    {
        public int WinningTeamIdOrTeamBId { get; set; }
        public string BettingTeamNameOrTeamA { get; set; }
        public string WinningTeamNameOrTeamB { get; set; }
        public int TeamAId { get; set; }
        public int TeamBId {get;set; }
        public decimal? NetAmountWon { get; set; }
        public DateTimeOffset MatchDate { get; set; }
        
        public bool IsBetDeleted { get; set; }
    }
}