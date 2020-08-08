namespace Pluto.BlogCore.Domain.DomainModels.Blog
{
	public class PostTag:Entity
	{
		public long PostId { get; set; }
		public Post Post { get; set; }

		public long TagId { get; set; }
		public Tag Tag { get; set; }


		public PostTag(long postId, long tagId)
		{
			PostId = postId;
			TagId = tagId;
		}

		public PostTag(Post post, Tag tag)
		{
			Post = post;
			Tag = tag;
		}

		public PostTag()
		{
			
		}
	}
}