namespace Pluto.BlogCore.Application.ResourceModels
{
	public class AuthorResourceModel
	{

		public AuthorResourceModel()
		{
			
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public AuthorResourceModel(string openId, string thirdOpenId)
		{
			OpenId = openId;
			ThirdOpenId = thirdOpenId;
		}

		public string OpenId { get; set; }
		public string ThirdOpenId { get; set; }

		public string UserName { get; set; }
	}
}