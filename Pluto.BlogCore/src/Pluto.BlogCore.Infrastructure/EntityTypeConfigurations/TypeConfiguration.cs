using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pluto.BlogCore.Domain.DomainModels.Blog;

namespace Pluto.BlogCore.Infrastructure.EntityTypeConfigurations
{
	public class PostConfiguration:IEntityTypeConfiguration<Post>
	{
		public void Configure(EntityTypeBuilder<Post> builder)
		{
			builder.ToTable("Post");
			builder.HasKey(x => x.Id);
			builder.HasOne(x => x.Category)
			       .WithMany(z => z.Posts)
			       .HasForeignKey(x=>x.CategoryId)
			       .IsRequired(false)
			       .OnDelete(DeleteBehavior.SetNull);
		}
	}
	
	
	public class CategoryConfiguration:IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable("Category");
			builder.HasKey(x => x.Id); // 与post关系post中已经配置了 不需要再次配置
		}
	}
	
	public class TagConfiguration:IEntityTypeConfiguration<Tag>
	{
		public void Configure(EntityTypeBuilder<Tag> builder)
		{
			builder.ToTable("Tag");
			builder.HasKey(x => x.Id);
		}
	}
	
	public class PostTagConfiguration:IEntityTypeConfiguration<PostTag>
	{
		public void Configure(EntityTypeBuilder<PostTag> builder)
		{
			builder.ToTable("PostTag");
			builder.HasKey(x => x.Id);
			
			builder.HasOne(pt => pt.Post)
			       .WithMany(p => p.PostTags)
			       .HasForeignKey(pt => pt.PostId)
			       .OnDelete(DeleteBehavior.Cascade)
			       .IsRequired(false);

			builder.HasOne(pt => pt.Tag)
			       .WithMany(t => t.PostTags)
			       .HasForeignKey(pt => pt.TagId)
			       .OnDelete(DeleteBehavior.Cascade)
			       .IsRequired(false);

			
		}
	}
}