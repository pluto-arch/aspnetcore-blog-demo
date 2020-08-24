using System;
using Newtonsoft.Json;

namespace Pluto.BlogCore.Application.HttpServices.Models
{
	public class YuqueDocModel
	{
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("slug")]
		public string Slug { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("title")]
		public string Title { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("description")]
		public string Description { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("user_id")]
		public string UserId { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("public")]
		public bool IsPublic { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("status")]
		public int Status { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("content_updated_at")]
		public DateTime ContentUpdatedAt { get; set; }
	}
}