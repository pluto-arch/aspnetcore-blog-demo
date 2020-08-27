using System.Collections.Generic;
using System.Threading.Tasks;
using Pluto.BlogCore.Application.ResourceModels;
using Pluto.BlogCore.Domain.DomainModels.Blog;
using PlutoData.Collections;

namespace Pluto.BlogCore.Application.Queries.Interfaces
{
	public interface IPostQueries
	{
		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="keyWord"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		Task<IPagedList<PostListItemModel>> GetListAsync(string keyWord, int pageIndex = 1, int pageSize = 20);

		/// <summary>
		/// 查询详情
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<PostListItemModel> GetAsync(long id);

		/// <summary>
		/// 获取类目
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		CategoryModel GetPostCategory(long? categoryId);

		/// <summary>
		/// 根据平台类型查询
		/// </summary>
		/// <param name="platform"></param>
		/// <param name="dataId"></param>
		/// <returns></returns>
		Task<PostListItemModel> GetByPlatformAsync(EnumPlatform platform, string dataId);


		/// <summary>
		/// 获取所有分类
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<CategoryModel>> GetCategorysAsync();
		/// <summary>
		/// 获取分类
		/// </summary>
		/// <returns></returns>
		CategoryModel GetCategoryByNameAsync(string displayName);
	}
}