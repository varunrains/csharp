using System;
using System.Threading.Tasks;

namespace IplServerSide.Core.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        IUserRepository Users { get; }
        IMatchRepository Matches { get; }
         IBetRepository Bets { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}