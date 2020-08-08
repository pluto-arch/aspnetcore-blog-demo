using System;
using AutoMapper;
using Pluto.BlogCore.Application.Commands.Models;
using Pluto.BlogCore.Domain.DomainModels.Blog;


namespace Pluto.BlogCore.Application
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuthorModel, Author>()
                .ConstructUsing(x=>new Author(x.OpenId,x.Name,x.Avatar));
        }
    }
}