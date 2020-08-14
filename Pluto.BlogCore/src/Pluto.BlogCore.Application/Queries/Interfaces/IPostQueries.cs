using System.Collections.Generic;
using System.Threading.Tasks;
using Pluto.BlogCore.Application.ResourceModels;
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
	}
}