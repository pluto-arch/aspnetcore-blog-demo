namespace Pluto.BlogCore.Application.Commands.Models
{
	public class AuthorModel
	{
		/// <summary>
		/// openid
		/// </summary>
		public string OpenId { get; private set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string ThirdOpenId { get; private set; }

		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public AuthorModel(string openId, string thirdOpenId)
		{
			OpenId = openId;
			ThirdOpenId = thirdOpenId;
		}

		public AuthorModel()
		{
			
		}
	}
}