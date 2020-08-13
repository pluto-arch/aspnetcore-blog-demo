using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
            CreateMap<AuthorModel, Author>()
                .ConstructUsing(x=>new Author(x.OpenId,x.Name,x.Avatar));

            CreateMap<Post, PostListItemModel>()
                .ForMember(x=>x.Id,o=>o.MapFrom(z=>z.Id))
                .ForMember(x=>x.Category,o=>o.MapFrom(z=>MapPostCategory(z.Category)))
                .ForMember(x=>x.Author,o=>o.MapFrom(z=>MapPostAuthor(z.Author)))
                .ForMember(x=>x.Title,o=>o.MapFrom(z=>z.Title))
                .ForMember(x=>x.Summary,o=>o.MapFrom(z=>z.Summary))
                .ForMember(x=>x.CreateTime,o=>o.MapFrom(z=>z.CreateTime))
                .ForMember(x=>x.Tags,o=>o.MapFrom(z=>MapPostTag(z.PostTags)));
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
            return new AuthorResourceModel(author.OpenId,author.Name);
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