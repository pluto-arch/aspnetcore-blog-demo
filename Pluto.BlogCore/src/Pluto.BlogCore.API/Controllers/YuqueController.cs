using System;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pluto.BlogCore.API.Models;
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
    [Route("third_auth")]
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
                OpenId = userInfo.data.id,
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = "",
                PlatformOpenId = userInfo.data.id,
                PlatformName = userInfo.data.name
            };
            await _mediator.Send(command);
            return Redirect(callbacks);
        }


        /// <summary>
        /// 获取用户的语雀文档
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse<object>> GetDocument(string userid,string accessToken)
        {
            var repos =await _yuQueAppService.GetUserRepos(userid, accessToken);
            return ApiResponse<object>.Success(repos);
        }
        
    }
}