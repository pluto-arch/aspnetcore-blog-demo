using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pluto.BlogCore.API.Models.Requests
{
	/// <summary>
	/// 创建Post
	/// </summary>
	public class CreatePostRequest : IValidatableObject
	{
		/// <summary>
		/// 邮箱
		/// </summary>
		[Required(ErrorMessage = "标题不能为空"),StringLength(300,ErrorMessage = "标题必须在300字以内")]
		public string Title { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Required(ErrorMessage = "密码不能为空"),StringLength(1000,ErrorMessage = "摘要字数必须是1000字以内")]
		public string Summary { get; set; }


		/// <summary>
		/// 类目id
		/// </summary>
		public long? CategoryId { get; set; }

		/// <summary>
		/// 标签
		/// </summary>
		public string[] Tags { get; set; }

		/// <summary>
		/// 是否直接提交
		/// </summary>
		public bool IsSubmit { get; set; }
		
		/// <summary>Determines whether the specified object is valid.</summary>
		/// <param name="validationContext">The validation context.</param>
		/// <returns>A collection that holds failed-validation information.</returns>
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (Tags.Length > 5)
			{
				yield return new ValidationResult(
				                                  "标签不能超过5个！",
				                                  new[] {nameof(Tags)});
			}
		}
	}
}