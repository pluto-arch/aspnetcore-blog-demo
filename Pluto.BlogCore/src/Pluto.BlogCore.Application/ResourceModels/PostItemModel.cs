using System.Collections.Generic;

namespace Pluto.BlogCore.Application.ResourceModels
{
	public class PostItemModel:PostListItemModel
	{
		public string HtmlContent { get; set; }

		public string MarkDownContent { get; set; }

		public List<string> Comments { get; set; }
	}
}