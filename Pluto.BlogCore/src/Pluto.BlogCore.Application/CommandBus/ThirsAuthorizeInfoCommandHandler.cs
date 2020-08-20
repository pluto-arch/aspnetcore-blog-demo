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
    public class ThirsAuthorizeInfoCommandHandler:IRequestHandler<CreateThirsAuthorizeInfoCommand,bool>
    {
        private readonly IUnitOfWork<PlutoBlogCoreDbContext> _unitOfWork;
        private readonly IRepository<ThirsAuthorizeInfo> _repository;

        public ThirsAuthorizeInfoCommandHandler(IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetBaseRepository<ThirsAuthorizeInfo>();
        }


        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<bool> Handle(CreateThirsAuthorizeInfoCommand request, CancellationToken cancellationToken)
        {
            var model = await _repository.GetFirstOrDefaultAsync(predicate:x => x.PlatformOpenId == request.PlatformOpenId,cancellationToken:cancellationToken);
            if (model==null)
            {
                await _repository.InsertAsync(new ThirsAuthorizeInfo
                {
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    PlatformType = request.PlatformType,
                    OpenId = request.OpenId,
                    AccessToken = request.AccessToken,
                    RefreshToken = request.RefreshToken,
                    Expired = request.Expired,
                    PlatformOpenId = request.PlatformOpenId
                },cancellationToken);
            } else
            {
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