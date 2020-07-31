using System;
using System.Collections.Generic;
using System.Linq;
using Pluto.BlogCore.Application.Queries.Interfaces;
using Pluto.BlogCore.Application.ResourceModels;
using Pluto.BlogCore.Domain.DomainModels.Account;
using Pluto.BlogCore.Domain.IRepositories;
using Pluto.BlogCore.Infrastructure;
using PlutoData.Collections;
using PlutoData.Interface;


//  ===================
// 2020-03-24
//  ===================

namespace Pluto.BlogCore.Application.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class UserQueries: IUserQueries
    {

        private readonly IRepository<UserEntity> _userRepository;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UserQueries(IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork)
        {
            _userRepository = unitOfWork.GetRepository<IUserRepository>();
        }


        /// <inheritdoc />
        public IPagedList<UserItemModel> GetUsers()
        {
            var pageList= _userRepository.GetPagedList<UserItemModel>(x => new UserItemModel{UserName=x.UserName,Email=x.Email},pageIndex:1,pageSize:20);
            return pageList;
        }

        /// <inheritdoc />
        public object GetUser(object key)
        {
            return _userRepository.Find(key);
        }
    }
}