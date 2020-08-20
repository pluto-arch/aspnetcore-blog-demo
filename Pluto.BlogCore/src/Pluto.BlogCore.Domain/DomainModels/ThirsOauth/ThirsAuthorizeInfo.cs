using System;

namespace Pluto.BlogCore.Domain.DomainModels.ThirsOauth
{
    public class ThirsAuthorizeInfo:BaseEntity<int>
    {
        /// <summary>
        /// 平台类型
        /// </summary>
        public EnumPlatformType PlatformType { get; set; }
        
        /// <summary>
        /// 本系统用户标识
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 平台授权accesstoken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 平台用户标识
        /// </summary>
        public string PlatformOpenId { get; set; }

        /// <summary>
        /// 平台用户名
        /// </summary>
        public string PlatformName { get; set; }
        
        /// <summary>
        /// 平台刷新token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 平台授权token 过期时间
        /// </summary>
        public DateTime? Expired { get; set; }

        
    }
}