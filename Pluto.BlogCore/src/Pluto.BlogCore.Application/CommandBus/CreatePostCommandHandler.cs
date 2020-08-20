using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Domain.DomainModels.Blog;
using Pluto.BlogCore.Domain.IRepositories.Blog;
using Pluto.BlogCore.Infrastructure;
using Pluto.BlogCore.Infrastructure.Providers;
using PlutoData.Interface;

namespace Pluto.BlogCore.Application.CommandBus
{
	public class CreatePostCommandHandler
		: IRequestHandler<CreatePostCommand, bool>
	{
		private IUnitOfWork<PlutoBlogCoreDbContext> _uow;
		private readonly IMapper _mapper;

		public CreatePostCommandHandler(IUnitOfWork<PlutoBlogCoreDbContext> uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}


		/// <summary>Handles a request</summary>
		/// <param name="request">The request</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Response from the request</returns>
		public async Task<bool> Handle(CreatePostCommand request, CancellationToken cancellationToken)
		{
			var postRep = _uow.GetRepository<IPostRepository>();
			var category = GetPostCategory(request.CategoryId);
			var entity = new Post(
			                      request.Title,
			                      request.Summary,
			                      category,
			                      request.IsAutoPhblish ? EnumPostStatus.Auditing : EnumPostStatus.Draft,
			                      _mapper.Map<Author>(request.Author));
			entity.Id = EntityIdGenerateProvider.GenerateLongId();
			if (request.Tags!=null&&request.Tags.Length > 0)
			{
				var postTags = SaveTags(request.Tags, cancellationToken);
				entity.AddTags(postTags);
			}

			await postRep.InsertAsync(entity, cancellationToken);
			return true;
		}

		

		#region private method
		/// <summary>
		/// 处理tag
		/// </summary>
		/// <param name="tags"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private List<Tag> SaveTags(string[] tags, CancellationToken cancellationToken)
		{
			var result = new List<Tag>();
			var tagRep = _uow.GetRepository<ITagRepository>();
			foreach (var tag in tags)
			{
				var exist = tagRep.GetFirstOrDefault(predicate:x=>x.TagName==tag);
				if (exist==null)
				{
					var insertTag = new Tag(tag);
					tagRep.Insert(insertTag);
					result.Add(insertTag);
				}
				else
				{
					result.Add(exist);
				}
				
			}
			return result;
		}
		/// <summary>
		/// 获取类目
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		/// <exception cref="InvalidDataException"></exception>
		private Category GetPostCategory(long? categoryId)
		{
			if (categoryId.HasValue)
			{
				var categoryRep = _uow.GetRepository<ICategoryRepository>();
				var category = categoryRep.Find(categoryId.Value);
				if (category == null)
				{
					throw new InvalidDataException($"类型不正确：{categoryId}");
				}

				return category;
			}

			return null;
		}

		#endregion
	}
}