using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pluto.BlogCore.API.Models;
using Pluto.BlogCore.API.Models.Requests;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Application.Queries.Interfaces;
using Pluto.BlogCore.Infrastructure.Providers;

namespace Pluto.BlogCore.API.Controllers
{
	[Route("api/category")]
	public class CategoryController:ApiBaseController<CategoryController>
	{
		private readonly IPostQueries _postQueries;
		public CategoryController(
			IMediator mediator, 
			ILogger<CategoryController> logger, 
			EventIdProvider eventIdProvider,
			IPostQueries postQueries) : base(mediator, logger, eventIdProvider)
		{
			_postQueries = postQueries;
		}


		/// <summary>
		/// 获取全部分类
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var list = await _postQueries.GetCategorysAsync();
			return Ok(ApiResponse.SuccessData(list));
		}
		
		/// <summary>
		/// 新建分类
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Post(CreateCategoryRequest request)
		{
			var category = _postQueries.GetCategoryByNameAsync(request.DisplayName);
			if (category!=null)
			{
				return BadRequest(ApiResponse.ErrorData("名称重复"));
			}
			var req = new CreateCategoryCommand(request.DisplayName);
		    var res= await _mediator.Send(req);
		    return Ok(ApiResponse.SuccessData(res));
		}
		
		
	}
}