using System.Threading.Tasks;

namespace Pluto.BlogCore.Application.Queries.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public interface IYuqueAuthQueries
	{
		Task<(string userId,string token)> GetUserWithTokenAsync(string openid);
	}
}