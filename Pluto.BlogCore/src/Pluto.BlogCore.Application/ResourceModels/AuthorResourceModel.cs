namespace Pluto.BlogCore.Application.ResourceModels
{
	public class AuthorResourceModel
	{

		public AuthorResourceModel()
		{
			
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public AuthorResourceModel(string openId, string authorName)
		{
			OpenId = openId;
			AuthorName = authorName;
		}

		public string OpenId { get; set; }

		public string AuthorName { get; set; }
	}
}