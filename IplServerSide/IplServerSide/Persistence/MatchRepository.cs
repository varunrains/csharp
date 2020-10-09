using IplServerSide.Core.Repositories;
using IplServerSide.Models;

namespace IplServerSide.Persistence
{
    public class MatchRepository  :Repository<Match>,IMatchRepository
    {
        public MatchRepository(BettingContext bettingContext) : base(bettingContext)
        {
        }
    }
}