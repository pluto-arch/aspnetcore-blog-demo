using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

using Pluto.BlogCore.Application.Attributes;

using System.Runtime.Serialization;
using Pluto.BlogCore.Application.Commands.Models;

namespace Pluto.BlogCore.Application.Commands
{
    /// <summary>
    /// 创建账户
    /// </summary>
    public class CreatePostCommand : BaseCommand,IRequest<bool>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; private set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public AuthorModel Author { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// 是否直接发布
        /// </summary>
        public bool IsAutoPhblish { get; set; } = false;
        
        public CreatePostCommand(string title, string summary, long? categoryId, AuthorModel author, string[] tags, bool isAutoPhblish)
        {
            Title = title;
            Summary = summary;
            CategoryId = categoryId;
            Author = author;
            IsAutoPhblish = isAutoPhblish;
            if (tags.Length>5)
            {
                throw new IndexOutOfRangeException("标签不能多于五个");
            }
            Tags = tags;
        }
    }
}