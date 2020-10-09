using IplServerSide.Core.Repositories;
using IplServerSide.Models;
using System;
using System.Linq;

namespace IplServerSide.Persistence
{
    public class UserRepository :Repository<User>, IUserRepository
    {
        private readonly BettingContext _bettingContext;
        public UserRepository(BettingContext bettingContext) : base(bettingContext)
        {
            _bettingContext = bettingContext;
        }

        public User ValidateUser(string userName, string password)
        {
            return _bettingContext.Users.FirstOrDefault(user =>
                user.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                && user.PassKey == password);
        }
    }
}