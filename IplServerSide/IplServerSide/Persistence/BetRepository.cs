using IplServerSide.Core.Repositories;
using IplServerSide.Models;

namespace IplServerSide.Persistence
{
    public class BetRepository : Repository<Bet>, IBetRepository
    {
        public BetRepository(BettingContext bettingContext) : base(bettingContext)
        {
        }
    }
}