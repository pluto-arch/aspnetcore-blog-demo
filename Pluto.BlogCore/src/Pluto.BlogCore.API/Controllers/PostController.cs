using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pluto.BlogCore.API.Models;
using Pluto.BlogCore.API.Models.Requests;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Application.HttpServices;
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
		
		public PostController(
			IMediator mediator, 
			ILogger<PostController> logger, 
			EventIdProvider eventIdProvider, 
			IPostQueries postQueries) : base(mediator, logger, eventIdProvider)
		{
			_postQueries = postQueries;
		}
		
		/// <summary>
		/// 获取列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Get(string keyWord,[Required,Range(1,int.MaxValue)]int pageIndex,[Required,Range(1,100)]int pageSize)
		{
			var list = await _postQueries.GetListAsync(keyWord,pageIndex,pageSize);
			return Ok(ApiResponse.SuccessData(list));
		}
		
		
		/// <summary>
		/// 获取详情
		/// </summary>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<IActionResult> Get([Required,Range(1,Int32.MaxValue)]long id)
		{
			if (id<=0)
			{
				return BadRequest(ApiResponse.ErrorData("请求参数不正确"));
			}
			var model= await _postQueries.GetAsync(id);
			return Ok(ApiResponse.SuccessData(model));
		}
		
		
		/// <summary>
		/// 创建post
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Post(CreatePostRequest request)
		{
			var command=new CreatePostCommand(
			                                  request.Title,
			                                  request.Summary,
			                                  request.CategoryId,
			                                  new AuthorModel("admin","admin"), 
			                                  request.Tags,
			                                  request.IsSubmit);
			var response= await _mediator.Send(command);
			return Ok(ApiResponse.SuccessData(response));
		}
		
		
	}
}