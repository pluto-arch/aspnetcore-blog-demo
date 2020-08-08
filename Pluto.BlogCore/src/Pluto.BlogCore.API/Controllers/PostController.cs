using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pluto.BlogCore.API.Models;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Application.Commands.Models;
using Pluto.BlogCore.Infrastructure.Providers;

namespace Pluto.BlogCore.API.Controllers
{
	[ApiController]
	[Route("api/posts")]
	public class PostController:ApiBaseController<PostController>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="logger"></param>
		/// <param name="eventIdProvider"></param>
		public PostController(
			IMediator mediator, 
			ILogger<PostController> logger, 
			EventIdProvider eventIdProvider) : base(mediator, logger, eventIdProvider)
		{
		}

		/// <summary>
		/// 创建post
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<ApiResponse<bool>> Post()
		{
			var command=new CreatePostCommand(
			                                  "测试哈哈哈",
			                                  "测试哈哈哈测试哈哈哈测试哈哈哈测试哈哈哈",
			                                  null,
			                                  new AuthorModel("admin","admin","admin"), 
			                                  new []{"1","2","3","4"},
			                                  false);
			var response= await _mediator.Send(command);
			if (response)
			{
				return ApiResponse<bool>.Success(true);
			}
			return ApiResponse<bool>.Fail();
		}
		
		
	}
}