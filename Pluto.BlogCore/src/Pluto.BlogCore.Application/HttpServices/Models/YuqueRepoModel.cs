using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluto.BlogCore.Application.HttpServices.Models
{
    /// <summary>
    /// 语雀知识库model
    /// </summary>
    public class YuqueRepoModel
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// 类型 [Book - 文档]
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// 仓库路径
        /// </summary>
        [JsonProperty("slug")]
        public string Slug { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 所属的团队/用户编号
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// 创建人 User Id
        /// </summary>
        [JsonProperty("creator_id")]
        public string CreatorId { get; set; }
        /// <summary>
        /// 是否公开 [true - 公开, false - 私密]
        /// </summary>
        [JsonProperty("public")]
        public bool Public { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("items_count")]
        public int ItemCount { get; set; }
        /// <summary>
        /// 喜欢数量
        /// </summary>
        [JsonProperty("likes_count")]
        public string LikeCount { get; set; }
        /// <summary>
        /// 订阅数量
        /// </summary>
        [JsonProperty("watches_count")]
        public string WatchCount { get; set; }
        /// <summary>
        /// 仓库完整路径 user.login/book.slug
        /// </summary>
        [JsonProperty("namespace")]
        public string NameSpace { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("user")]
        public YuqueUseInfoModel User { get; set; }
    }
}