using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pluto.BlogCore.Application.Attributes;
using Pluto.BlogCore.Infrastructure;
using PlutoData.Interface;

namespace Pluto.BlogCore.Application.Behaviors
{
	/// <summary>
	/// 自动保存
	/// </summary>
	public class AutoSaveChangeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{

		private readonly IUnitOfWork<PlutoBlogCoreDbContext> _uow;
		private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
		
		public AutoSaveChangeBehavior(IUnitOfWork<PlutoBlogCoreDbContext> uow, ILogger<LoggingBehavior<TRequest, TResponse>> logger)
		{
			_uow = uow;
			_logger = logger;
		}
		
		
		
		/// <summary>
		/// Pipeline handler. Perform any additional behavior and await the <paramref name="next" /> delegate as necessary
		/// </summary>
		/// <param name="request">Incoming request</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
		/// <returns>Awaitable task returning the <typeparamref name="TResponse" /></returns>
		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			var type = request.GetType();
			var attr = type.GetCustomAttribute(typeof(DisableAutoSaveChangeAttribute), true) as DisableAutoSaveChangeAttribute;
			if (attr != null)
			{
				return await next();
			}
			var response = await next();
			if (_uow.DbContext.ChangeTracker.Entries().Any(x => x.State != EntityState.Unchanged))
			{
				await _uow.SaveChangesAsync(cancellationToken);
			}
			return response;
		}
	}
}