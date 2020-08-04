using System.Collections.Generic;

namespace Pluto.BlogCore.Domain.DomainModels.Blog
{
	/// <summary>
	/// 标签
	/// </summary>
	public class Tag : BaseEntity<long>
	{
		/// <summary>
		/// 标签名称
		/// </summary>
		public string TagName { get; set; }

		/// <summary>
		/// 展示名称
		/// </summary>
		public string DisplayName { get; set; }
		
		/// <summary>
		/// 导航属性-标签
		/// </summary>
		public IReadOnlyCollection<PostTag> PostTags { get; set; }

	}
}