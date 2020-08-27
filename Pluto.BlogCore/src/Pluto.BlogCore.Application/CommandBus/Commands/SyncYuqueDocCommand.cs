using System;
using MediatR;
using Pluto.BlogCore.Application.Commands.Models;
using Pluto.BlogCore.Domain.DomainModels.Blog;

namespace Pluto.BlogCore.Application.Commands
{
    public class SyncYuqueDocCommand:IRequest<bool>
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public SyncYuqueDocCommand(string title, string summary, long? categoryId, AuthorModel author, string htmlContent, string markdownContent,  EnumContentFormat format, string slug)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new Exception("标题不能为空");
            }

            if (string.IsNullOrEmpty(summary)||summary.Length>=300)
            {
                throw new Exception("摘要必须300字以内");
            }

            Title = title;
            Summary = summary;
            CategoryId = categoryId;
            Author = author;
            HtmlContent = htmlContent;
            MarkdownContent = markdownContent;
            Type = EnumPostType.Content;
            Format = format;
            Platform = EnumPlatform.Yuque;
            PlatformId = slug;
        }

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

        public string HtmlContent { get; set; }

        public string MarkdownContent { get; set; }

        public string Link { get; set; }

        public EnumPostType Type { get; set; }

        public EnumContentFormat Format { get; set; }

        public EnumPlatform Platform { get; set; }

        public string PlatformId { get; set; }

    }
}