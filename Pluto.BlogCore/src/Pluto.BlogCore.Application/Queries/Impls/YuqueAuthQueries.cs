using System.Threading.Tasks;
using Pluto.BlogCore.Application.Queries.Interfaces;
using Pluto.BlogCore.Domain.DomainModels.Yuque;
using Pluto.BlogCore.Infrastructure;
using PlutoData.Interface;

namespace Pluto.BlogCore.Application.Queries.Impls
{
	public class YuqueAuthQueries:IYuqueAuthQueries
	{
		private readonly IUnitOfWork<PlutoBlogCoreDbContext> _unitOfWork;
		
		public YuqueAuthQueries(IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		
		/// <summary>
		/// 获取accesstoken
		/// </summary>
		/// <param name="openid"></param>
		/// <returns></returns>
		public async Task<(string userId,string token)> GetUserWithTokenAsync(string openid)
		{
			var rep = _unitOfWork.GetBaseRepository<YuqueAuthInfo>();
			var entity =await rep.GetFirstOrDefaultAsync(predicate:x => x.OpenId == openid);
			if (entity==null)
			{
				return default;
			}
			return (entity.PlatformOpenId,entity.AccessToken);
		}
	}
}