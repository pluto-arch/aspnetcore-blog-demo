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
		
		
		
		/// <summary>Determines whether the specified object is valid.</summary>
		/// <param name="validationContext">The validation context.</param>
		/// <returns>A collection that holds failed-validation information.</returns>
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			return default;
			// if (UserName.Length < 4)
			// {
			// 	yield return new ValidationResult(
			// 	                                  "用户名长度不够！",
			// 	                                  new[] {nameof(UserName)});
			// }
			//
			// if (Password.Length < 4)
			// {
			// 	yield return new ValidationResult(
			// 	                                  "密码长度不够！",
			// 	                                  new[] {nameof(Password)});
			// }
		}
	}
}