using System;
using Newtonsoft.Json;

namespace Pluto.BlogCore.Application.HttpServices.Models
{
    public class YuqueDocDetailModel
    {
        /// <summary>
        /// doc id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// doc 标识
        /// </summary>
        [JsonProperty("slug")]
        public string Slug { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 格式化类型 lake , markdown
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// html 内容
        /// </summary>
        [JsonProperty("body_html")]
        public string BodyHtml { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [JsonProperty("published_at",NullValueHandling = NullValueHandling.Ignore)]
        public DateTime PublishedAt { get; set; }
        
        [JsonProperty("created_at")]
        private DateTime CodeModel2 { set {
            if (PublishedAt==default)
            {
                PublishedAt = value;
            }
        } }

        /// <summary>
        /// 摘要
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// 是否是公开文档
        /// </summary>
        [JsonProperty("public")]
        public bool IsPublic { get; set; }
    }
}