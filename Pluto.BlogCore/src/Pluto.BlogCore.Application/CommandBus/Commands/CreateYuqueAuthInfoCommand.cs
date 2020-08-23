using System;
using MediatR;
using Pluto.BlogCore.Domain.DomainModels.ThirsOauth;

namespace Pluto.BlogCore.Application.Commands
{
    public class CreateYuqueAuthInfoCommand:IRequest<bool>
    {
        
        /// <summary>
        /// 本系统用户标识
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 平台授权accesstoken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 平台刷新token
        /// </summary>
        public string RefreshToken { get; set; }
        
        public string PlatformName { get; set; }

        /// <summary>
        /// 平台授权token 过期时间
        /// </summary>
        public DateTime? Expired { get; set; }

        /// <summary>
        /// 平台用户标识
        /// </summary>
        public string PlatformOpenId { get; set; }
    }
}