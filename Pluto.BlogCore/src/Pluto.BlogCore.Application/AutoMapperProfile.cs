using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Pluto.BlogCore.Application.ResourceModels;
using Pluto.BlogCore.Domain.DomainModels.Blog;
using PlutoData.Collections;
using AuthorModel = Pluto.BlogCore.Application.Commands.Models.AuthorModel;


namespace Pluto.BlogCore.Application
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            #region inpur
            CreateMap<AuthorModel, Author>()
                .ConstructUsing(x=>new Author(x.OpenId,x.ThirdOpenId));
            

            #endregion

            #region output

            CreateMap<Category, CategoryModel>()
                .ForMember(x=>x.Id,o=>o.MapFrom(s=>s.Id))
                .ForMember(x=>x.DisplayName,o=>o.MapFrom(s=>s.DisplayName));

            CreateMap<Post, PostListItemModel>()
                .ForMember(x=>x.Id,o=>o.MapFrom(z=>z.Id))
                .ForMember(x=>x.Category,o=>o.MapFrom(z=>MapPostCategory(z.Category)))
                .ForMember(x=>x.Author,o=>o.MapFrom(z=>MapPostAuthor(z.Author)))
                .ForMember(x=>x.Title,o=>o.MapFrom(z=>z.Title))
                .ForMember(x=>x.Summary,o=>o.MapFrom(z=>z.Summary))
                .ForMember(x=>x.CreateTime,o=>o.MapFrom(z=>z.CreateTime))
                .ForMember(x=>x.Tags,o=>o.MapFrom(z=>MapPostTag(z.PostTags)));
            
            
            CreateMap<Post, PostItemModel>()
                .ForMember(x=>x.Id,o=>o.MapFrom(z=>z.Id))
                .ForMember(x=>x.Category,o=>o.MapFrom(z=>MapPostCategory(z.Category)))
                .ForMember(x=>x.Author,o=>o.MapFrom(z=>MapPostAuthor(z.Author)))
                .ForMember(x=>x.Title,o=>o.MapFrom(z=>z.Title))
                .ForMember(x=>x.Summary,o=>o.MapFrom(z=>z.Summary))
                .ForMember(x=>x.CreateTime,o=>o.MapFrom(z=>z.CreateTime))
                .ForMember(x=>x.HtmlContent,o=>o.MapFrom(z=>z.HtmlContent))
                .ForMember(x=>x.MarkDownContent,o=>o.MapFrom(z=>z.MarkdownContent))
                .ForMember(x=>x.Tags,o=>o.MapFrom(z=>MapPostTag(z.PostTags)));
            

            #endregion
            
        }

        private IEnumerable<TagModel> MapPostTag(IEnumerable<PostTag> postTags)
        {
            if (postTags==null||!postTags.Any())
            {
                return null;
            }
            var res=new List<TagModel>();
            foreach (var tags in postTags)
            {
                if (tags.Tag!=null)
                {
                    res.Add(new TagModel(tags.Tag.Id,tags.Tag.DisplayName));
                }
            }

            return res;
        }

        private AuthorResourceModel MapPostAuthor(Author author)
        {
            if (author==null)
            {
                return null;
            }
            return new AuthorResourceModel(author.OpenId,author.ThirdOpenid);
        }

        private CategoryModel MapPostCategory(Category category)
        {
            if (category==null)
            {
                return null;
            }
            return new CategoryModel(category.Id,category.DisplayName);
        }
    }
}