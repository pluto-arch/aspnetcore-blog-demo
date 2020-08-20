using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pluto.BlogCore.Application.HttpServices.Models;
using Pluto.BlogCore.Application.Options;

namespace Pluto.BlogCore.Application.HttpServices
{
    public class YuQueAppService
    {
        public HttpClient Client { get; }
        private readonly ILogger<YuQueAppService> _logger;
        private readonly YuqueOption _options;
        public YuQueAppService(HttpClient client, ILogger<YuQueAppService> logger,IOptions<YuqueOption> options)
        {
            Client = client;
            _logger = logger;
            _options = options.Value;
        }

        public string GetOauthAuthorizeUrl(string callback)
        {
            var callbacks = HttpUtility.UrlEncode(callback);
            return
                $"{_options.AuthUrl}authorize?client_id={_options.ClientId}&scope=repo:read,doc:read&redirect_uri={_options.RedirectUrl}&state={callbacks}&response_type=code";
        }


        /// <summary>
        /// 换取accesstoken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<YuQueAccessTokenModel> GetAccessToken(string code)
        {
            var request = new YuQueAccessTokenRequest
            {
                ClientId = _options.ClientId, ClientSecret = _options.ClientSecret, Code = code
            };
            var content=new StringContent(JsonConvert.SerializeObject(request));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response=await Client.PostAsync($"{_options.AuthUrl}token",content);
            var responseText = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"语雀获取token返回数据：{responseText}");
            return JsonConvert.DeserializeObject<YuQueAccessTokenModel>(responseText);
        }

        public async Task<YuQueUseInfoModel> GetUserInfoAsync(string accessToken)
        {
            Client.DefaultRequestHeaders.Add("X-Auth-Token",accessToken);
            Client.DefaultRequestHeaders.Add("User-Agent",_options.AppName);
            var response=await Client.GetAsync($"{_options.ApiUrl}user");
            var responseText = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"语雀获取用户信息返回数据：{responseText}");
            return JsonConvert.DeserializeObject<YuQueUseInfoModel>(responseText);
        }
    }
}