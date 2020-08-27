using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Application.Queries.Interfaces;
using Pluto.BlogCore.Application.ResourceModels;
using Pluto.BlogCore.Domain.DomainModels.Blog;
using Pluto.BlogCore.Domain.DomainModels.Yuque;
using Pluto.BlogCore.Infrastructure;
using Pluto.BlogCore.Infrastructure.Providers;
using PlutoData.Interface;

namespace Pluto.BlogCore.Application.CommandBus
{
    public class YuqueCommandHandler
        :IRequestHandler<CreateYuqueAuthInfoCommand,bool>,
        IRequestHandler<SyncYuqueDocCommand,bool>
    {
        private readonly IUnitOfWork<PlutoBlogCoreDbContext> _unitOfWork;
        private readonly IPostQueries _postQueries;
        private readonly IMapper _mapper;
        public YuqueCommandHandler(
            IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork, 
            IPostQueries postQueries,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _postQueries = postQueries;
            _mapper = mapper;
        }


        /// <summary>创建/修改语雀用户信息</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<bool> Handle(CreateYuqueAuthInfoCommand request, CancellationToken cancellationToken)
        {
            var _repository = _unitOfWork.GetBaseRepository<YuqueAuthInfo>();
            var model = await _repository.GetFirstOrDefaultAsync(predicate:x => x.OpenId == request.OpenId,cancellationToken:cancellationToken);
            if (model==null)
            {
                await _repository.InsertAsync(new YuqueAuthInfo
                {
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    OpenId = request.OpenId,
                    AccessToken = request.AccessToken,
                    RefreshToken = request.RefreshToken,
                    Expired = request.Expired,
                    PlatformOpenId = request.PlatformOpenId,
                    Avator = request.Avator
                },cancellationToken);
            } else
            {
                model.PlatformOpenId = request.PlatformOpenId;
                model.AccessToken = request.AccessToken;
                model.PlatformName = request.PlatformName;
                model.Expired = request.Expired;
                model.RefreshToken = request.RefreshToken;
                model.ModifyTime=DateTime.Now;
                model.Avator = request.Avator;
            }
            return true;
        }

        /// <summary>语雀稿件同步</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<bool> Handle(SyncYuqueDocCommand request, CancellationToken cancellationToken)
        {
            var rep = _unitOfWork.GetBaseRepository<Post>();
            var category = _postQueries.GetPostCategory(request.CategoryId);
            var model =await rep.GetFirstOrDefaultAsync(predicate: x
                                                             => x.PlatformInfo!=null&&x.PlatformInfo.Platform == request.Platform
                                                                                    && x.PlatformInfo.PlatformId == request.PlatformId,cancellationToken:cancellationToken);
            if (model==null)
            {
                var entity= new Post(
                                 request.Title,
                                 request.Summary,
                                 _mapper.Map<Category>(category),
                                 EnumPostStatus.Passed,
                                 request.Type,
                                 request.HtmlContent,
                                 request.MarkdownContent,
                                 request.Link,
                                 new ContentPlatformInfo
                                 {
                                     Platform = request.Platform, Format = request.Format, PlatformId =request.PlatformId 
                                 },
                                 _mapper.Map<Author>(request.Author));
                entity.Id = EntityIdGenerateProvider.GenerateLongId();
                await rep.InsertAsync(entity,cancellationToken);
            }
            else
            {
                model.Title = request.Title;
                model.Summary = request.Summary;
                model.Category = _mapper.Map<Category>(category);
                model.HtmlContent = request.HtmlContent;
            }
            return true;
        }
    }
}