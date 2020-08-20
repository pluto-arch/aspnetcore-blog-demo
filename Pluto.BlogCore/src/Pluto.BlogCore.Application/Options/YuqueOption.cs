namespace Pluto.BlogCore.Application.Options
{
    public class YuqueOption
    {
        public const string Yuque = "YuqueOauth";
        /// <summary>
        /// 语雀创建的第三方应用名称
        /// </summary>
        public string AppName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        /// <summary>
        /// 语雀Oauth地址
        /// </summary>
        public string AuthUrl { get; set; }
        /// <summary>
        /// 语雀开放接口地址
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// 语雀用户点击同意授权后跳转地址
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 本地跳转地址
        /// 语雀授权完成后
        /// </summary>
        public string CallbackUrl { get; set; }
    }
}