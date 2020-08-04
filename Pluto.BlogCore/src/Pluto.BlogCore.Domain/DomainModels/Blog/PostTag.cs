namespace Pluto.BlogCore.Domain.DomainModels.Blog
{
	public class PostTag:BaseEntity<long>
	{
		public long PostId { get; set; }
		public Post Post { get; set; }

		public long TagId { get; set; }
		public Tag Tag { get; set; }
	}
}