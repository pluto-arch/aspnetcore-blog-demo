using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Pluto.BlogCore.Domain.SeedWork;

namespace Pluto.BlogCore.Domain.DomainModels.Blog
{
	public class Author:ValueObject
	{
		/// <summary>
		/// openid
		/// </summary>
		public string OpenId { get; private set; }

		/// <summary>
		/// 第三方openid
		/// </summary>
		public string ThirdOpenid { get; set; }

		/// <summary>
		/// 初始化 <see cref="Author"/> 的新实例
		/// </summary>
		public Author() { }

		public Author(string openId, string thirdOpenid)
		{
			OpenId = openId;
			ThirdOpenid = thirdOpenid;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return OpenId;
			yield return ThirdOpenid;
		}
	}
}