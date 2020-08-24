using System;

namespace Pluto.BlogCore.API.Models.Response
{
    public class YuqueRepoResponse
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 类型 [Book - 文档]
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否公开 [true - 公开, false - 私密]
        /// </summary>
        public bool Public { get; set; }
        /// <summary>
        /// 仓库完整路径 user.login/book.slug
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}