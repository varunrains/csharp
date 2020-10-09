using IplServerSide.Models;

namespace IplServerSide.Core.Repositories
{
    public interface IUserRepository:IRepository<User>
    {
        User ValidateUser(string userName, string password);
    }
}