using Pluto.BlogCore.Domain.DomainModels.Blog;
using Pluto.BlogCore.Domain.IRepositories.Blog;
using PlutoData;

namespace Pluto.BlogCore.Infrastructure.Repositories
{
	public class CategoryRepository:Repository<Category>,ICategoryRepository
	{
		
	}
}