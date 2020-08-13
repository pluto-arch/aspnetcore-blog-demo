using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pluto.BlogCore.API.Models;
using Pluto.BlogCore.API.Models.Requests;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Application.Queries.Interfaces;
using Pluto.BlogCore.Application.ResourceModels;
using Pluto.BlogCore.Infrastructure.Providers;
using PlutoData.Collections;
using AuthorModel = Pluto.BlogCore.Application.Commands.Models.AuthorModel;

namespace Pluto.BlogCore.API.Controllers
{
	[ApiController]
	[Route("api/posts")]
	public class PostController:ApiBaseController<PostController>
	{
		private readonly IPostQueries _postQueries;
		
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="logger"></param>
		/// <param name="eventIdProvider"></param>
		public PostController(
			IMediator mediator, 
			ILogger<PostController> logger, 
			EventIdProvider eventIdProvider, IPostQueries postQueries) : base(mediator, logger, eventIdProvider)
		{
			_postQueries = postQueries;
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<ApiResponse<IPagedList<PostListItemModel>>> Get(string keyWord,int pageIndex,int pageSize)
		{
			if (pageIndex<=0||pageSize<=0)
			{
				return ApiResponse<IPagedList<PostListItemModel>>.Fail("暂无数据");
			}
			var list = await _postQueries.GetListAsync(keyWord,pageIndex,pageSize);
			return ApiResponse<IPagedList<PostListItemModel>>.Success(list);
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