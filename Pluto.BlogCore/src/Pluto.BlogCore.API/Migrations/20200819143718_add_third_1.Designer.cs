﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pluto.BlogCore.Infrastructure;

namespace Pluto.BlogCore.API.Migrations
{
    [DbContext(typeof(PlutoBlogCoreDbContext))]
    [Migration("20200819143718_add_third_1")]
    partial class add_third_1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Pluto.BlogCore.Domain.DomainModels.Blog.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("ModifyTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Pluto.BlogCore.Domain.DomainModels.Blog.Post", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<long?>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime>("ModifyTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Id", "Title");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Pluto.BlogCore.Domain.DomainModels.Blog.PostTag", b =>
                {
                    b.Property<long>("PostId")
                        .HasColumnType("bigint");

                    b.Property<long>("TagId")
                        .HasColumnType("bigint");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("Pluto.BlogCore.Domain.DomainModels.Blog.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Pluto.BlogCore.Domain.DomainModels.ThirsOauth.ThirsAuthorizeInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime?>("Expired")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifyTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("OpenId")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("PlatformName")
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("PlatformOpenId")
                        .IsRequired()
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<string>("PlatformType")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.HasKey("Id");

                    b.HasIndex("OpenId", "PlatformOpenId");

                    b.ToTable("ThirsAuthorizeInfo");
                });

            modelBuilder.Entity("Pluto.BlogCore.Domain.DomainModels.Blog.Post", b =>
                {
                    b.HasOne("Pluto.BlogCore.Domain.DomainModels.Blog.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("Pluto.BlogCore.Domain.DomainModels.Blog.Author", "Author", b1 =>
                        {
                            b1.Property<long>("PostId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Avatar")
                                .HasColumnName("AuthorAvatar")
                                .HasColumnType("nvarchar(512)")
                                .HasMaxLength(512);

                            b1.Property<string>("Name")
                                .HasColumnName("AuthorName")
                                .HasColumnType("nvarchar(256)")
                                .HasMaxLength(256);

                            b1.Property<string>("OpenId")
                                .HasColumnName("AuthorOpenId")
                                .HasColumnType("nvarchar(256)")
                                .HasMaxLength(256);

                            b1.HasKey("PostId");

                            b1.ToTable("Post");

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });
                });

            modelBuilder.Entity("Pluto.BlogCore.Domain.DomainModels.Blog.PostTag", b =>
                {
                    b.HasOne("Pluto.BlogCore.Domain.DomainModels.Blog.Post", "Post")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Pluto.BlogCore.Domain.DomainModels.Blog.Tag", "Tag")
                        .WithMany("PostTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
