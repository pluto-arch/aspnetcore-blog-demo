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
		/// 名称
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// 头像
		/// </summary>
		public string Avatar { get; private set; }


		/// <summary>
		/// 初始化 <see cref="Author"/> 的新实例
		/// </summary>
		public Author() { }

		/// <summary>
		/// 初始化 <see cref="Author"/> 的新实例
		/// </summary>
		/// <param name="openId"></param>
		/// <param name="name"></param>
		/// <param name="avatar"></param>
		public Author(string openId, string name, string avatar)
		{
			OpenId = openId;
			Name = name;
			Avatar = avatar;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return OpenId;
			yield return Name;
			yield return Avatar;
		}
	}
}