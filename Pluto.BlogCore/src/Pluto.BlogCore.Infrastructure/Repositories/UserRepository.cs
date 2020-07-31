using Pluto.BlogCore.Domain.DomainModels.Account;
using Pluto.BlogCore.Domain.IRepositories;
using PlutoData;


namespace Pluto.BlogCore.Infrastructure.Repositories
{
    public class UserRepository:Repository<UserEntity>, IUserRepository
    {
    }
}