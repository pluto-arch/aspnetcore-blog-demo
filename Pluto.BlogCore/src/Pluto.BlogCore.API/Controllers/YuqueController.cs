using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pluto.BlogCore.API.Models;
using Pluto.BlogCore.API.Models.Response;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Application.Commands.Models;
using Pluto.BlogCore.Application.HttpServices;
using Pluto.BlogCore.Application.HttpServices.Models;
using Pluto.BlogCore.Application.Options;
using Pluto.BlogCore.Application.Queries.Interfaces;
using Pluto.BlogCore.Domain.DomainModels.ThirsOauth;
using Pluto.BlogCore.Infrastructure.Extensions;
using Pluto.BlogCore.Infrastructure.Providers;

namespace Pluto.BlogCore.API.Controllers
{
    [ApiController]
    [Route("yuque")]
    public class YuqueController : ApiBaseController<YuqueController>
    {
        private readonly YuQueAppService _yuQueAppService;
        private readonly YuqueOption _options;
        private readonly IYuqueAuthQueries _yuqueAuthQueries;

        public YuqueController(IMediator mediator,
                               ILogger<YuqueController> logger,
                               EventIdProvider eventIdProvider,
                               YuQueAppService yuQueAppService,
                               IOptions<YuqueOption> options, IYuqueAuthQueries yuqueAuthQueries) : base(mediator, logger, eventIdProvider)
        {
            _yuQueAppService = yuQueAppService;
            _yuqueAuthQueries = yuqueAuthQueries;
            _options = options.Value;
        }

        /// <summary>
        /// 获取语雀授权跳转地址
        /// </summary>
        /// <param name="callback">授权回调后的地址</param>
        /// <returns></returns>
        [HttpGet("yuque_auth_url")]
        public ApiResponse<string> GetYuqueAuthUrl(string callback)
        {
            var res = _yuQueAppService.GetOauthAuthorizeUrl(callback);
            return ApiResponse<string>.Success(res);
        }

        /// <summary>
        /// 语雀跳转回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet("yuque_auth_redirect")]
        public async Task<IActionResult> YuqueAuthRedirect(string code, string state)
        {
            var callbacks = _options.CallbackUrl;
            if (!string.IsNullOrEmpty(state)&&state.IsUrl())
            {
                callbacks = HttpUtility.UrlDecode(state);
            }
            var tokenResponse = await _yuQueAppService.GetAccessToken(code);
            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return Redirect(callbacks);
            }
            var userInfo = await _yuQueAppService.GetUserInfoAsync(tokenResponse.AccessToken);
            var command=new CreateYuqueAuthInfoCommand
            {
                OpenId = "PO11212112312",
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = "",
                PlatformOpenId = userInfo.Data.Id,
                PlatformName = userInfo.Data.Name
            };
            await _mediator.Send(command);
            return Redirect(callbacks);
        }


        /// <summary>
        /// 获取用户的语雀知识库
        /// </summary>
        /// <returns></returns>
        [HttpGet("repos")]
        public async Task<ApiResponse<IEnumerable<YuqueRepoResponse>>> GetRepo(int page)
        {
            if (page<=0)
            {
                return ApiResponse<IEnumerable<YuqueRepoResponse>>.Fail("页码错误");
            }
            string userid = "PO11212112312";
            var info =await _yuqueAuthQueries.GetUserWithTokenAsync(userid);
            if (string.IsNullOrEmpty(info.token))
            {
                return ApiResponse<IEnumerable<YuqueRepoResponse>>.Fail("获取token失败");
            }
            var repos =await _yuQueAppService.GetUserRepos(info.userId, info.token,page);
            var response = from repo in repos.Data
                           select new YuqueRepoResponse
                           {
                               Id = repo.Id,
                               Type = repo.Type,
                               Name = repo.Name,
                               Public = repo.Public,
                               NameSpace = repo.NameSpace,
                               CreatedAt =repo.CreatedAt 
                           };
            return ApiResponse<IEnumerable<YuqueRepoResponse>>.Success(response);
        }
        
        
        /// <summary>
        /// 获取用户的语雀知识库文档
        /// </summary>
        /// <param name="repoId">知识库id</param>
        /// <returns></returns>
        [HttpGet("repos/{repoId}/docs")]
        public async Task<ApiResponse<IEnumerable<YuqueDocModel>>> GetDocs(string repoId)
        {
            string userid = "PO11212112312";
            var info =await _yuqueAuthQueries.GetUserWithTokenAsync(userid);
            if (string.IsNullOrEmpty(info.token))
            {
                return ApiResponse<IEnumerable<YuqueDocModel>>.Fail("获取token失败");
            }
            var repos =await _yuQueAppService.GetRepoDocs(info.token, repoId);
            return ApiResponse<IEnumerable<YuqueDocModel>>.Success(repos.Data);
        }
        
        /// <summary>
        /// 同步用户的语雀知识库文档详情
        /// </summary>
        /// <param name="repoId">知识库id</param>
        /// <param name="slug">文档slug</param>
        /// <returns></returns>
        [HttpGet("repos/{repoId}/docs/{slug}")]
        public async Task<ApiResponse<object>> SyncDoc(string repoId,string slug)
        {
            string userid = "PO11212112312";
            var info =await _yuqueAuthQueries.GetUserWithTokenAsync(userid);
            if (string.IsNullOrEmpty(info.token))
            {
                return ApiResponse<object>.Fail("获取token失败");
            }
            var repos =await _yuQueAppService.GetRepoDoc(info.token, repoId,slug);
            
            
            
            return ApiResponse<object>.Success(repos.Data);
        }
    }
}