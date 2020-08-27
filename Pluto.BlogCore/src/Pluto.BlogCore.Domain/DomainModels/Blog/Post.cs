using System.Collections.Generic;
using System.Linq;

namespace Pluto.BlogCore.Domain.DomainModels.Blog
{
	/// <summary>
	/// 帖子
	/// </summary>
	public class Post : BaseEntity<long>
	{
		public Post()
		{
		}

		public Post(string title, string summary, Category category, EnumPostStatus status, Author author)
		{
			Title = title;
			Summary = summary;
			Category = category;
			Status = status;
			Author = author;
		}

		public Post(
			string title, 
			string summary, 
			Category category, 
			EnumPostStatus status, 
			EnumPostType postType, 
			string htmlContent, 
			string markdownContent, 
			string link, 
			ContentPlatformInfo platformInfo, 
			Author author)
		{
			Title = title;
			Summary = summary;
			Category = category;
			Status = status;
			PostType = postType;
			HtmlContent = htmlContent;
			MarkdownContent = markdownContent;
			Link = link;
			PlatformInfo = platformInfo;
			Author = author;
		}


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

		/// <summary>
		/// 状态
		/// </summary>
		public EnumPostStatus Status { get; set; }

		/// <summary>
		/// 类型
		/// </summary>
		public EnumPostType PostType { get; set; }

		/// <summary>
		/// html原始内容
		/// </summary>
		public string HtmlContent { get; set; }

		/// <summary>
		/// markodwn内容
		/// </summary>
		public string MarkdownContent { get; set; }

		/// <summary>
		/// 外链
		/// </summary>
		public string Link { get; set; }

		/// <summary>
		/// 平台信息
		/// </summary>
		public ContentPlatformInfo PlatformInfo { get; set; }

		/// <summary>
		/// 作者
		/// </summary>
		public Author Author { get; set; }

		/// <summary>
		/// 导航属性-标签
		/// </summary>
		public List<PostTag> PostTags { get; set; }


		public void Published()
		{
			this.Status = EnumPostStatus.Passed;
			//AddDomainEvent();
		}

		public void Rejected()
		{
			this.Status = EnumPostStatus.Unqualified;
			//AddDomainEvent();
		}

		public void AddTags(List<Tag> tags)
		{
			if (!tags.Any())
			{
				return;
			}
			this.PostTags ??= new List<PostTag>();
			var list = from t in tags
			           select new PostTag(this,t);
			this.PostTags.AddRange(list);
		}
	}
}