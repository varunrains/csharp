using IplServerSide.Core.Repositories;
using System.Threading.Tasks;

namespace IplServerSide.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BettingContext _bettingContext;
        public IUserRepository Users { get; }
        public IMatchRepository Matches { get; }
        public IBetRepository Bets { get; }
        public UnitOfWork(BettingContext bettingContext)
        {
            _bettingContext = bettingContext;
            Users = new UserRepository(_bettingContext);
            Matches = new MatchRepository(_bettingContext);
            Bets = new BetRepository(_bettingContext);
        }
        public void Dispose()
        {
            _bettingContext.Dispose();
        }

        
        public int Complete()
        {
           return _bettingContext.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
          return await _bettingContext.SaveChangesAsync();
        }
    }
}