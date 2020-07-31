﻿using PlutoData.Collections;
using System;
using System.Collections.Generic;
using Pluto.BlogCore.Application.ResourceModels;

//  ===================
// 2020-03-24
//  ===================

namespace Pluto.BlogCore.Application.Queries.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPagedList<UserItemModel> GetUsers();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetUser(object key);
    }
}