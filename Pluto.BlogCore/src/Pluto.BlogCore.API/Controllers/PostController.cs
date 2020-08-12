using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pluto.BlogCore.API.Models;
using Pluto.BlogCore.API.Models.Requests;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Application.ResourceModels;
using Pluto.BlogCore.Infrastructure.Providers;
using AuthorModel = Pluto.BlogCore.Application.Commands.Models.AuthorModel;

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
		/// 获取列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<ApiResponse<IEnumerable<PostListItemModel>>> Get()
		{
			return ApiResponse<IEnumerable<PostListItemModel>>.Fail();
		}
		
		
		/// <summary>
		/// 获取详情
		/// </summary>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<ApiResponse<PostItemModel>> Get(long id)
		{
			return ApiResponse<PostItemModel>.Fail();
		}
		
		
		/// <summary>
		/// 创建post
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<ApiResponse<bool>> Post(CreatePostRequest request)
		{
			var command=new CreatePostCommand(
			                                  request.Title,
			                                  request.Summary,
			                                  request.CategoryId,
			                                  new AuthorModel("admin","admin","admin"), 
			                                  request.Tags,
			                                  request.IsSubmit);
			var response= await _mediator.Send(command);
			if (response)
			{
				return ApiResponse<bool>.Success(true);
			}
			return ApiResponse<bool>.Fail();
		}
		
		
	}
}