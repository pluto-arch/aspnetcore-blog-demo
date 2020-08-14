using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pluto.BlogCore.API.Models;
using Pluto.BlogCore.API.Models.Requests;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Infrastructure.Providers;

namespace Pluto.BlogCore.API.Controllers
{
	[Route("api/category")]
	public class CategoryController:ApiBaseController<CategoryController>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="logger"></param>
		/// <param name="eventIdProvider"></param>
		public CategoryController(
			IMediator mediator, 
			ILogger<CategoryController> logger, 
			EventIdProvider eventIdProvider) : base(mediator, logger, eventIdProvider)
		{
		}


		/// <summary>
		/// 获取全部分类
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ApiResponse Get()
		{
			return ApiResponse.DefaultFail();
		}
		
		/// <summary>
		/// 新建分类
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<ApiResponse> Post(CreateCategoryRequest request)
		{
			var req = new CreateCategoryCommand(request.DisplayName);
		    var res= await _mediator.Send(req);
		    if (res)
		    {
			    return ApiResponse.DefaultSuccess();
		    }
			return ApiResponse.DefaultFail();
		}
		
		
	}
}