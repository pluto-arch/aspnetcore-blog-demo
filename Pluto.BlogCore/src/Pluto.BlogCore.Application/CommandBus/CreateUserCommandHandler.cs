using System;
using MediatR;
using Pluto.BlogCore.Application.Commands;
using System.Threading;
using System.Threading.Tasks;
using Pluto.BlogCore.Domain.DomainModels.Account;
using Pluto.BlogCore.Domain.IRepositories;
using Pluto.BlogCore.Infrastructure;
using Pluto.BlogCore.Infrastructure.Extensions;
using PlutoData.Interface;

namespace Pluto.BlogCore.Application.CommandBus
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {

        private readonly IMediator _mediator;
        private readonly IUnitOfWork<PlutoBlogCoreDbContext> _unitOfWork;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="unitOfWork"></param>
        public CreateUserCommandHandler(
            IMediator mediator, 
            IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _userRepository =unitOfWork.GetRepository<IUserRepository>();
        }


        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new UserEntity
            {
                UserName = request.UserName,
                Email= request.UserName+"@qq.com"
            };
            user.SetPasswordHash(request.Password);  // 有可能会注册领域事件
            _userRepository.Insert(user);

            // 如果要触发领域事件，
            await _mediator.DispatchDomainEventsAsync(_unitOfWork.DbContext);
            var res= await _unitOfWork.SaveChangesAsync(cancellationToken);
            return res>0;
        }
    }
}