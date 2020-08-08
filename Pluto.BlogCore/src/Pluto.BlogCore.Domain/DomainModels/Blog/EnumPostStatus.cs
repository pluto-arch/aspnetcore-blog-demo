namespace Pluto.BlogCore.Domain.DomainModels.Blog
{
	public enum EnumPostStatus
	{
		/// <summary>
		/// 审核通过
		/// </summary>
		Passed,
		/// <summary>
		/// 审核中
		/// </summary>
		Auditing,
		/// <summary>
		/// 草稿
		/// </summary>
		Draft,
		/// <summary>
		/// 不合格
		/// </summary>
		Unqualified
	}
}