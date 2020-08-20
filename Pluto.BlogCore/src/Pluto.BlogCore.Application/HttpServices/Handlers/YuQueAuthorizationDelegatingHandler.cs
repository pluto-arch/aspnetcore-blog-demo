using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Pluto.BlogCore.Application.HttpServices.Handlers
{
    public class YuQueAuthorizationDelegatingHandler:DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly ILogger<YuQueAuthorizationDelegatingHandler> _logger;

        public YuQueAuthorizationDelegatingHandler(
            IHttpContextAccessor httpContextAccesor, 
            ILogger<YuQueAuthorizationDelegatingHandler> logger)
        {
            _httpContextAccesor = httpContextAccesor;
            _logger = logger;
        }
        
        // protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        // {
        //     request.Version = new System.Version(2, 0);
        //     var authorizationHeader = _httpContextAccesor.HttpContext
        //                                                  .Request.Headers["Authorization"];
        //     if (!string.IsNullOrEmpty(authorizationHeader))
        //     {
        //         request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
        //     }
        //     var token = await GetToken();
        //     request.Headers.Authorization = new AuthenticationHeaderValue("X-Auth-Token", $"Bearer {token}");
        //     return await base.SendAsync(request, cancellationToken);
        // }
        // async Task<string> GetToken()
        // {
        //     const string ACCESS_TOKEN = "yuque_token";
        //     return await _httpContextAccesor.HttpContext
        //                                     .GetTokenAsync(ACCESS_TOKEN);
        // }
    }

}