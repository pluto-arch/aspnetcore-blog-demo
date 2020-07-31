﻿using System.ComponentModel.DataAnnotations;
using MediatR;

using Pluto.BlogCore.Application.Attributes;

using System.Runtime.Serialization;

namespace Pluto.BlogCore.Application.Commands
{
    /// <summary>
    /// 创建账户
    /// </summary>
    public class CreateUserCommand : IRequest<bool>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        public CreateUserCommand()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public CreateUserCommand(string userName, string password) : this()
        {
            UserName = userName;
            Password = password;
        }

    }
}