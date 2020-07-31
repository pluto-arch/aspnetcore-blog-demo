using System;
using Pluto.BlogCore.Domain.DomainModels.Account;
using PlutoData.Interface;


namespace Pluto.BlogCore.Domain.IRepositories
{
    public interface IUserRepository: IRepository<UserEntity>
    {
        
    }
}