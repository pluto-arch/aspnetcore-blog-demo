using System;
using System.Collections.Generic;
using System.Net;
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
using Pluto.BlogCore.Domain.DomainModels.Blog;

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
                $"{_options.AuthUrl}authorize?client_id={_options.ClientId}&scope=repo,doc&redirect_uri={_options.RedirectUrl}&state={callbacks}&response_type=code";
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
            ProcessResponse(response.StatusCode);
            var responseText = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"语雀获取token返回数据：{responseText}");
            return JsonConvert.DeserializeObject<YuQueAccessTokenModel>(responseText);
        }

        /// <summary>
        /// 获取用语雀户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<YuqueBaseModel<YuQueUseInfoModel>> GetUserInfoAsync(string accessToken)
        {
            SetYuqueAuthHeader(accessToken);
            var response=await Client.GetAsync($"{_options.ApiUrl}user");
            ProcessResponse(response.StatusCode);
            var responseText = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"语雀获取用户信息返回数据：{responseText}");
            return JsonConvert.DeserializeObject<YuqueBaseModel<YuQueUseInfoModel>>(responseText);
        }


        /// <summary>
        /// 获取语雀用户知识库
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accessToken"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<YuqueBaseModel<IEnumerable<YuqueRepoModel>>> GetUserRepos(string id,string accessToken,int page=1)
        {
            SetYuqueAuthHeader(accessToken);
            var response=await Client.GetAsync($"{_options.ApiUrl}users/{id}/repos?type=all&offset={page-1}");
            ProcessResponse(response.StatusCode);
            var responseText = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"语雀获取用户信息返回数据：{responseText}");
            return JsonConvert.DeserializeObject<YuqueBaseModel<IEnumerable<YuqueRepoModel>>>(responseText);
        }

        /// <summary>
        /// 获取知识库中的文档
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="idOrNameSpace">存储库id或者namespace</param>
        /// <returns></returns>
        public async Task<YuqueBaseModel<IEnumerable<YuqueDocModel>>> GetRepoDocs(string accessToken,string idOrNameSpace)
        {
            SetYuqueAuthHeader(accessToken);
            var response=await Client.GetAsync($"{_options.ApiUrl}repos/{idOrNameSpace}/docs");
            ProcessResponse(response.StatusCode);
            var responseText = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"语雀获取用户信息返回数据：{responseText}");
            return JsonConvert.DeserializeObject<YuqueBaseModel<IEnumerable<YuqueDocModel>>>(responseText);
        }
        
        /// <summary>
        /// 获取知识库中的文档详情
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="idOrNameSpace"></param>
        /// <param name="slug"></param>
        /// <returns></returns>
        public async Task<YuqueBaseModel<object>> GetRepoDoc(string accessToken,string idOrNameSpace,string slug)
        {
            SetYuqueAuthHeader(accessToken);
            var response=await Client.GetAsync($"{_options.ApiUrl}repos/{idOrNameSpace}/docs/{slug}?format=0");
            ProcessResponse(response.StatusCode);
            var responseText = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"语雀获取用户信息返回数据：{responseText}");
            return JsonConvert.DeserializeObject<YuqueBaseModel<object>>(responseText);
        }
        
        

        #region private method
        private void SetYuqueAuthHeader(string accessToken)
        {
            Client.DefaultRequestHeaders.Add("X-Auth-Token", accessToken);
            Client.DefaultRequestHeaders.Add("User-Agent", _options.AppName);
        }
        
        private void ProcessResponse(HttpStatusCode responseStatusCode)
        {
            switch (responseStatusCode)
            {
                case HttpStatusCode.OK:
                    return;
                case HttpStatusCode.Forbidden:
                    throw new Exception("权限不足");
                case HttpStatusCode.BadRequest:
                    throw new Exception("请求的参数不正确，或缺少必要信息");
                case HttpStatusCode.NotFound:
                    throw new Exception("数据不存在，或未开放");
                case HttpStatusCode.Unauthorized:
                    throw new Exception("未授权");
                case HttpStatusCode.InternalServerError:
                    throw new Exception("服务器异常");
                default:
                    throw new Exception("请求异常");
            }
        }
        

        #endregion
        
    }
}