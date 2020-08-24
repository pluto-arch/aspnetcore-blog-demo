using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Domain.DomainModels.ThirsOauth;
using Pluto.BlogCore.Infrastructure;
using PlutoData.Interface;

namespace Pluto.BlogCore.Application.CommandBus
{
    public class YuqueCommandHandler:IRequestHandler<CreateYuqueAuthInfoCommand,bool>
    {
        private readonly IUnitOfWork<PlutoBlogCoreDbContext> _unitOfWork;
        private readonly IRepository<YuqueAuthInfo> _repository;

        public YuqueCommandHandler(IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetBaseRepository<YuqueAuthInfo>();
        }


        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<bool> Handle(CreateYuqueAuthInfoCommand request, CancellationToken cancellationToken)
        {
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
                    PlatformOpenId = request.PlatformOpenId
                },cancellationToken);
            } else
            {
                model.PlatformOpenId = request.PlatformOpenId;
                model.AccessToken = request.AccessToken;
                model.PlatformName = request.PlatformName;
                model.Expired = request.Expired;
                model.RefreshToken = request.RefreshToken;
                model.ModifyTime=DateTime.Now;
            }
            return true;
        }
    }
}