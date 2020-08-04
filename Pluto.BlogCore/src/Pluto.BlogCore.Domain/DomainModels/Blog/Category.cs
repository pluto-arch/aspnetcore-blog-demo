using System.Collections.ObjectModel;

namespace Pluto.BlogCore.Domain.DomainModels.Blog
{
	/// <summary>
	/// 类目
	/// </summary>
	public class Category:BaseEntity<long>
	{
		/// <summary>
		/// 分类名称
		/// </summary>
		public string CategoryName { get; set; }

		/// <summary>
		/// 展示名称
		/// </summary>
		public string DisplayName { get; set; }

		#region 导航属性
		public ReadOnlyCollection<Post> Posts { get; set; }
		#endregion
	}
}