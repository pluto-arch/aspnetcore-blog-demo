using System.ComponentModel.DataAnnotations;

namespace Pluto.BlogCore.API.Models.Requests
{
	public class CreateCategoryRequest
	{
		[Required(ErrorMessage = "显示名称必填"),StringLength(10,ErrorMessage = "长度不能大于10")]
		public string DisplayName { get; set; }
	}
}