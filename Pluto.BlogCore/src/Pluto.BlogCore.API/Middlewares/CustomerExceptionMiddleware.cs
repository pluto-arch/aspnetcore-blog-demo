﻿using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Pluto.BlogCore.Infrastructure.Providers;
using Serilog.Context;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pluto.BlogCore.API.Models;

namespace Pluto.BlogCore.API.Middlewares
{
	/// <summary>
	/// 
	/// </summary>
	public static class ApplicationBuilderExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static IApplicationBuilder UseExceptionProcess(this IApplicationBuilder app)
		{
			app.UseMiddleware<CustomerExceptionHandler>();
			return app;
		}
	}

	internal class CustomerExceptionHandler
	{
		private readonly RequestDelegate _next;

		private readonly EventIdProvider _eventIdProvider;

		private readonly ILogger _logger;


		public CustomerExceptionHandler(EventIdProvider eventIdProvider, RequestDelegate next,
		                                ILogger<CustomerExceptionHandler> logger)
		{
			_eventIdProvider = eventIdProvider;
			_next = next;
			_logger = logger;
		}


		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next.Invoke(httpContext);
			}
			catch (Exception e)
			{
				_logger.LogError(_eventIdProvider.EventId, e, $"{httpContext.Request.Path} has an error. {e.Message}");
				await HandlerExceptionAsync(httpContext, e);
			}
		}


		private async Task HandlerExceptionAsync(HttpContext context, Exception e)
		{
			context.Response.ContentType = "application/json;charset=utf-8";
			context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
			var apiResponse = ApiResponse.ErrorData(e.Message);
			var serializerResult = JsonConvert.SerializeObject(apiResponse);
			await context.Response.WriteAsync(serializerResult);
		}
	}
}