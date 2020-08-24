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
using Pluto.BlogCore.Application.Options;
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

        public YuqueController(IMediator mediator,
                               ILogger<YuqueController> logger,
                               EventIdProvider eventIdProvider,
                               YuQueAppService yuQueAppService,
                               IOptions<YuqueOption> options) : base(mediator, logger, eventIdProvider)
        {
            _yuQueAppService = yuQueAppService;
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
                PlatformOpenId = userInfo.Data.AccountId,
                PlatformName = userInfo.Data.Name
            };
            await _mediator.Send(command);
            return Redirect(callbacks);
        }


        /// <summary>
        /// 获取用户的语雀知识库
        /// </summary>
        /// <returns></returns>
        [HttpPost("repos")]
        public async Task<ApiResponse<IEnumerable<YuqueRepoResponse>>> GetRepo(string userid,string accessToken,int page)
        {
            var repos =await _yuQueAppService.GetUserRepos(userid, accessToken,page);
            var response = from repo in repos.Repos
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
        
    }
}