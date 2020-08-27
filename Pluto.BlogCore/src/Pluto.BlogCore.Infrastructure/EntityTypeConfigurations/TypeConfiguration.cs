using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pluto.BlogCore.Domain.DomainModels.Blog;
using Pluto.BlogCore.Domain.DomainModels.Yuque;

namespace Pluto.BlogCore.Infrastructure.EntityTypeConfigurations
{
	public class PostConfiguration:IEntityTypeConfiguration<Post>
	{
		public void Configure(EntityTypeBuilder<Post> builder)
		{
			builder.ToTable("Post");
			builder.HasKey(x => x.Id);
			builder.Property(x=>x.Id).ValueGeneratedNever(); // 数据库不生成值
			builder.HasIndex(x => new {x.Id,x.Title});
			builder.HasOne(x => x.Category)
			       .WithMany(z => z.Posts)
			       .IsRequired(false)
			       .OnDelete(DeleteBehavior.SetNull);

			builder.OwnsOne(x => x.Author,a =>
			{
				a.WithOwner();
				a.Property(x => x.OpenId)
				 .HasMaxLength(256)
				 .HasColumnName("OpenId");
				a.Property(x => x.ThirdOpenid)
				 .HasMaxLength(256)
				 .HasColumnName("ThirdOpenid");
			});
			builder.OwnsOne(x => x.PlatformInfo,a =>
			{
				a.WithOwner();
				a.Property(x => x.PlatformId)
				 .HasMaxLength(64)
				 .HasColumnName("PlatformId");
				
				a.Property(x => x.Platform)
				 .HasColumnType("nvarchar(32)")
				 .HasConversion<string>()
				 .HasColumnName("Platform");
				
				a.Property(x => x.Format)
				 .HasColumnType("nvarchar(32)")
				 .HasConversion<string>()
				 .HasColumnName("Format");
			});

			builder.Property(x => x.Title)
			       .HasMaxLength(300)
			       .IsRequired(true);

			builder.Property(x => x.Summary)
			       .HasMaxLength(1000)
			       .IsRequired(true);

			builder.Property(x => x.HtmlContent)
			       .HasColumnType("text");
			builder.Property(x => x.MarkdownContent)
			       .HasColumnType("text");
			builder.Property(x => x.Link)
			       .HasMaxLength(1000);

			builder.Property(x => x.Status)
			       .HasColumnType("nvarchar(32)")
			       .HasConversion<string>();
			
			builder.Property(x => x.PostType)
			       .HasColumnType("nvarchar(16)")
			       .HasConversion<string>();

			builder.Property(x => x.CreateTime)
			       .HasDefaultValueSql("GETDATE()");
			builder.Property(x => x.ModifyTime)
			       .HasDefaultValueSql("GETDATE()");
		}
	}
	
	
	public class CategoryConfiguration:IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable("Category");
			builder.HasKey(x => x.Id); // 与post关系post中已经配置了 不需要再次配置
			builder.Property(x => x.CategoryName)
			       .HasMaxLength(64)
			       .IsRequired(true);
			builder.Property(x => x.DisplayName)
			       .HasMaxLength(128)
			       .IsRequired(true);
			builder.Property(x => x.CreateTime)
			       .HasDefaultValueSql("GETDATE()");
			builder.Property(x => x.ModifyTime)
			       .HasDefaultValueSql("GETDATE()");
		}
	}
	
	public class TagConfiguration:IEntityTypeConfiguration<Tag>
	{
		public void Configure(EntityTypeBuilder<Tag> builder)
		{
			builder.ToTable("Tag");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.TagName)
			       .HasMaxLength(32)
			       .IsRequired(true);
			builder.Property(x => x.CreateTime)
			       .HasDefaultValueSql("GETDATE()");
			builder.Property(x => x.ModifyTime)
			       .HasDefaultValueSql("GETDATE()");
		}
	}
	
	public class PostTagConfiguration:IEntityTypeConfiguration<PostTag>
	{
		public void Configure(EntityTypeBuilder<PostTag> builder)
		{
			builder.ToTable("PostTag");
			builder.HasKey(x=>new {x.PostId,x.TagId});
			
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
	
	
	public class ThirsAuthorizeInfoConfiguration:IEntityTypeConfiguration<YuqueAuthInfo>
	{
		public void Configure(EntityTypeBuilder<YuqueAuthInfo> builder)
		{
			builder.ToTable("YuqueAuth");
			builder.HasKey(x => x.Id);
			builder.HasIndex(x => new {x.OpenId,x.PlatformOpenId});
			
			builder.Property(x => x.OpenId)
			       .HasMaxLength(300)
			       .IsRequired(true);

			builder.Property(x => x.AccessToken)
			       .HasMaxLength(1024)
			       .IsRequired(true);
			builder.Property(x => x.RefreshToken)
			       .HasMaxLength(1024)
			       .IsRequired(true);
			builder.Property(x => x.PlatformOpenId)
			       .HasMaxLength(1024)
			       .IsRequired(true);
			builder.Property(x => x.Expired)
			       .IsRequired(false);

			builder.Property(x => x.PlatformName).HasMaxLength(300);

			builder.Property(x => x.CreateTime)
			       .HasDefaultValueSql("GETDATE()");
			builder.Property(x => x.ModifyTime)
			       .HasDefaultValueSql("GETDATE()");
		}
	}
	
	
	
	
}