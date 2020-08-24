using Newtonsoft.Json;

namespace Pluto.BlogCore.Application.HttpServices.Models
{
	public class YuqueBaseModel<T>
	{
		[JsonProperty("data")]
		public T Data { get; set; }
	}
}