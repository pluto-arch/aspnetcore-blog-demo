﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Pluto.BlogCore.API.Models;
using Pluto.BlogCore.Infrastructure;


namespace Pluto.BlogCore.API.Filters
{
	/// <summary>
	/// model 验证过滤器
	/// </summary>
	public class ModelValidateFilter : IActionFilter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				List<string> errors = new List<string>();
				foreach (var item in context.ModelState.Values)
				{
					foreach (var error in item.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}

				context.Result =
					new JsonResult(ApiResponse.ErrorData(string.Join("|", errors)));
			}
		}
	}
}