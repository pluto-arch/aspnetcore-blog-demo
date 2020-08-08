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
		public string Name { get; private set; }
		/// <summary>
		/// 头像
		/// </summary>
		public string Avatar { get; private set; }

		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public AuthorModel(string openId, string name, string avatar)
		{
			OpenId = openId;
			Name = name;
			Avatar = avatar;
		}
	}
}