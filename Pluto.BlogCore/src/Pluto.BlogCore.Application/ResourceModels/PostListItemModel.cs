using System;
using System.Collections.Generic;


namespace Pluto.BlogCore.Application.ResourceModels
{
	public class PostListItemModel
	{
		public long Id { get; set; }
		
		public string Title { get; set; }

		public string Summary { get; set; }

		public DateTime CreateTime { get; set; }

		public AuthorResourceModel Author { get; set; }

		public CategoryModel Category { get; set; }

		public List<TagModel> Tags { get; set; }
	}
}