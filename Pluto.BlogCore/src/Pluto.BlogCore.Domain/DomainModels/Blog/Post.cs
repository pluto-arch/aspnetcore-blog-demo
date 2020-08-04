using System.Collections.Generic;

namespace Pluto.BlogCore.Domain.DomainModels.Blog
{
	/// <summary>
	/// 帖子
	/// </summary>
	public class Post : BaseEntity<long>
	{
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 摘要
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		/// 导航属性-所属分类
		/// </summary>
		public Category Category { get; set; }

		public long? CategoryId { get; set; }
		
		/// <summary>
		/// 导航属性-标签
		/// </summary>
		public IReadOnlyCollection<PostTag> PostTags { get; set; }
	}
}