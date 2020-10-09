namespace IplServerSide.Dtos
{
    public class BetDto
    {
        public int BettingTeamIdOrTeamAId { get; set; }
        public int BetAmount { get; set; }
        public int MatchId { get; set; }
        public int BetId { get; set; }
    }
}