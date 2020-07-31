using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Domain.IRepositories;
using Pluto.BlogCore.Infrastructure;
using PlutoData.Interface;



namespace Pluto.BlogCore.Application.CommandBus
{
    public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,bool>
    {

        private readonly IMediator _mediator;

        private readonly IUnitOfWork<PlutoBlogCoreDbContext> _unitOfWork;

        public DeleteUserCommandHandler(
            IMediator mediator, IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }


        /// <inheritdoc />
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var rep = _unitOfWork.GetRepository<IUserRepository>();
            rep.Delete(request.Id);
            return (await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken)) > 0;
        }
    }
}