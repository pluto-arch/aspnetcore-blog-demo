using Newtonsoft.Json;

namespace Pluto.BlogCore.Application.HttpServices.Models
{
    public class YuqueUseInfoModel
    {
        /// <summary>
        /// 用户资料编号
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// 类型 [User - 用户, Group - 团队]
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// 企业空间编号
        /// </summary>
        [JsonProperty("space_id")]
        public string SpaceId { get; set; }
        /// <summary>
        /// 用户账户编号
        /// </summary>
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
        /// <summary>
        /// 用户个人路径
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 头像 URL
        /// </summary>
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 仓库数量
        /// </summary>
        [JsonProperty("books_count")]
        public int BooksCount { get; set; }
        /// <summary>
        /// 公开仓库数量
        /// </summary>
        [JsonProperty("public_books_count")]
        public int PublicBooksCount { get; set; }
    }
    
}