using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pluto.BlogCore.Infrastructure.Providers;


namespace Pluto.BlogCore.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ApiController]
    public class ApiBaseController<T> : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        internal readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        internal readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        internal readonly EventIdProvider _eventIdProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="eventIdProvider"></param>
        public ApiBaseController(IMediator mediator, ILogger<T> logger, EventIdProvider eventIdProvider)
        {
            _mediator = mediator;
            _logger = logger;
            _eventIdProvider = eventIdProvider;
        }

        protected string UserId
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return User.FindFirst("sub").Value;
                }

                return string.Empty;
            }
        }
    }
}