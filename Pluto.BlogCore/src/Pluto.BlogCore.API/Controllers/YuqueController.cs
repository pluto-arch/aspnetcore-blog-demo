using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
using Pluto.BlogCore.Domain.DomainModels.Blog;
using Pluto.BlogCore.Infrastructure.Extensions;
using Pluto.BlogCore.Infrastructure.Providers;

namespace Pluto.BlogCore.API.Controllers
{
    [ApiController]
    [Route("yuque")]
    [Authorize]
    public class YuqueController : ApiBaseController<YuqueController>
    {
        private readonly YuQueAppService _yuQueAppService;
        private readonly YuqueOption _options;
        private readonly IYuqueAuthQueries _yuqueAuthQueries;
        private readonly IPostQueries _postQueries;

        public YuqueController(IMediator mediator,
                               ILogger<YuqueController> logger,
                               EventIdProvider eventIdProvider,
                               YuQueAppService yuQueAppService,
                               IOptions<YuqueOption> options,
                               IYuqueAuthQueries yuqueAuthQueries,
                               IPostQueries postQueries) : base(mediator, logger, eventIdProvider)
        {
            _yuQueAppService = yuQueAppService;
            _yuqueAuthQueries = yuqueAuthQueries;
            _postQueries = postQueries;
            _options = options.Value;
        }

        /// <summary>
        /// 获取语雀授权跳转地址
        /// </summary>
        /// <param name="callback">授权回调后的地址</param>
        /// <returns></returns>
        [HttpGet("yuque_auth_url")]
        public IActionResult GetYuqueAuthUrl(string callback)
        {
            if (string.IsNullOrEmpty(callback))
            {
                return BadRequest(ApiResponse.ErrorData("回调地址不能为空"));
            }
            var res = _yuQueAppService.GetOauthAuthorizeUrl(callback);
            return Ok(ApiResponse.SuccessData(res));
        }

        /// <summary>
        /// 语雀跳转回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet("yuque_auth_redirect")]
        [AllowAnonymous]
        public async Task<IActionResult> YuqueAuthRedirect(string code, string state)
        {
            var callbacks = _options.CallbackUrl;
            if (!string.IsNullOrEmpty(state) && state.IsUrl())
            {
                callbacks = HttpUtility.UrlDecode(state);
            }

            var userId = new Uri(callbacks).Query.GetPara("userId");
            var tokenResponse = await _yuQueAppService.GetAccessToken(code);
            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return Redirect(callbacks);
            }

            var userInfo = await _yuQueAppService.GetUserInfoAsync(tokenResponse.AccessToken);
            var command = new CreateYuqueAuthInfoCommand
            {
                OpenId = userId,
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = "",
                PlatformOpenId = userInfo.Data.Id,
                PlatformName = userInfo.Data.Name,
                Avator = userInfo.Data.AvatarUrl
            };
            await _mediator.Send(command);
            return Redirect(callbacks);
        }


        /// <summary>
        /// 获取用户的语雀知识库
        /// </summary>
        /// <returns></returns>
        [HttpGet("repos")]
        public async Task<IActionResult> GetRepo(int page)
        {
            if (page <= 0)
            {
                return BadRequest(ApiResponse.ErrorData("页码必须大于0"));
            }

            string userid = UserId;
            var info = await _yuqueAuthQueries.GetUserWithTokenAsync(userid);
            if (string.IsNullOrEmpty(info.token))
            {
                return Unauthorized(ApiResponse.ErrorData("token不存在"));
            }

            var repos = await _yuQueAppService.GetUserRepos(info.userId, info.token, page);
            var response = from repo in repos.Data
                           select new YuqueRepoResponse
                           {
                               Id = repo.Id,
                               Type = repo.Type,
                               Name = repo.Name,
                               Public = repo.Public,
                               NameSpace = repo.NameSpace,
                               CreatedAt = repo.CreatedAt
                           };
            return Ok(ApiResponse.SuccessData(response));
        }


        /// <summary>
        /// 获取用户的语雀知识库文档
        /// </summary>
        /// <param name="repoId">知识库id</param>
        /// <returns></returns>
        [HttpGet("repos/{repoId}/docs")]
        public async Task<IActionResult> GetDocs(string repoId)
        {
            string userid = UserId;
            var info = await _yuqueAuthQueries.GetUserWithTokenAsync(userid);
            if (string.IsNullOrEmpty(info.token))
            {
                return Unauthorized(ApiResponse.ErrorData("token不存在,请重新授权"));
            }

            var repos = await _yuQueAppService.GetRepoDocs(info.token, repoId);
            return Ok(ApiResponse.SuccessData(repos.Data));
        }

        /// <summary>
        /// 同步用户的语雀知识库文档详情
        /// </summary>
        /// <param name="repoId">知识库id</param>
        /// <param name="slug">文档slug</param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("repos/{repoId}/docs/{slug}")]
        public async Task<IActionResult> SyncDoc(string repoId, string slug, long? categoryId)
        {
            string userid = UserId;
            var info = await _yuqueAuthQueries.GetUserWithTokenAsync(userid);
            if (string.IsNullOrEmpty(info.token))
            {
                return Unauthorized(ApiResponse.ErrorData("token不存在,请重新授权"));
            }

            var repos = await _yuQueAppService.GetRepoDoc(info.token, repoId, slug);
            var doc = repos.Data;
            if (!doc.IsPublic)
            {
                return BadRequest(ApiResponse.ErrorData("此文档为非公开文档，暂不支持同步"));
            }
            var author = new AuthorModel(userid, doc.UserId);
            var res= await _mediator.Send(new SyncYuqueDocCommand(doc.Title, doc.Description, categoryId, author, doc.BodyHtml.Replace("<!doctype html>",string.Empty),
                                                         string.Empty, EnumContentFormat.Lake, doc.Slug));
            return Ok(ApiResponse.SuccessData(res));
        }
    }
}