using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Pluto.BlogCore.Application.Commands;
using Pluto.BlogCore.Domain.DomainModels.Blog;
using Pluto.BlogCore.Infrastructure;
using PlutoData.Interface;

namespace Pluto.BlogCore.Application.CommandBus
{
	public class CreateCategoryCommandHandler:IRequestHandler<CreateCategoryCommand,bool>
	{
		private readonly IUnitOfWork<PlutoBlogCoreDbContext> _unitOfWork;
		private readonly IRepository<Category> _repository;
		
		public CreateCategoryCommandHandler(IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_repository = unitOfWork.GetBaseRepository<Category>();
		}
		
		
		/// <summary>Handles a request</summary>
		/// <param name="request">The request</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Response from the request</returns>
		public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
		{
			await _repository.InsertAsync(new Category(request.DisplayName,request.DisplayName),cancellationToken);
			return true;
		}
	}
}