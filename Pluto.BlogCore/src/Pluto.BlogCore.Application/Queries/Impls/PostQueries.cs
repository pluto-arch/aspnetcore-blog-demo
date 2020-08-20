using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pluto.BlogCore.Application.Queries.Interfaces;
using Pluto.BlogCore.Application.ResourceModels;
using Pluto.BlogCore.Domain.DomainModels.Blog;
using Pluto.BlogCore.Domain.IRepositories.Blog;
using Pluto.BlogCore.Infrastructure;
using PlutoData.Collections;
using PlutoData.Extensions;
using PlutoData.Interface;

namespace Pluto.BlogCore.Application.Queries.Impls
{
    public class PostQueries : IPostQueries
    {
        private readonly IUnitOfWork<PlutoBlogCoreDbContext> _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostQueries(IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _postRepository = _unitOfWork.GetRepository<IPostRepository>();
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IPagedList<PostListItemModel>> GetListAsync(string keyWord,
                                                                      int pageIndex = 1,
                                                                      int pageSize = 20)
        {
            Expression<Func<Post, bool>> expression = x => true;
            if (!string.IsNullOrEmpty(keyWord))
            {
                expression = expression.And(x => EF.Functions.Like(x.Title, $"%{keyWord}%"));
            }

            var res = await _postRepository.GetPagedListAsync(predicate: expression, pageIndex: pageIndex,
                                                              include: x => x
                                                                            .Include(i => i.PostTags)
                                                                            .ThenInclude(s => s.Tag),
                                                              pageSize: pageSize);

            var pageList = PagedList.From(res, x => _mapper.Map<IEnumerable<PostListItemModel>>(x));
            return pageList;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PostListItemModel> GetAsync(long id)
        {
            var res = await _postRepository.GetFirstOrDefaultAsync(predicate: x => x.Id == id,
                                                                   include: x => x.Include(a => a.Category)
                                                                                  .Include(b => b.PostTags)
                                                                                  .ThenInclude(t => t.Tag));
            return _mapper.Map<PostListItemModel>(res);
        }
    }
}