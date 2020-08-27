using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IMapper _mapper;

        public PostQueries(IUnitOfWork<PlutoBlogCoreDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var _postRepository = _unitOfWork.GetRepository<IPostRepository>();
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
            var _postRepository = _unitOfWork.GetRepository<IPostRepository>();
            var res = await _postRepository.GetFirstOrDefaultAsync(predicate: x => x.Id == id,
                                                                   include: x => x.Include(a => a.Category)
                                                                                  .Include(b => b.PostTags)
                                                                                  .ThenInclude(t => t.Tag));
            return _mapper.Map<PostListItemModel>(res);
        }

        /// <summary>
        /// 获取类目
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public CategoryModel GetPostCategory(long? categoryId)
        {
            var _postRepository = _unitOfWork.GetRepository<ICategoryRepository>();
            if (categoryId.HasValue)
            {
                var category = _postRepository.Find(categoryId.Value);
                if (category == null)
                {
                    throw new InvalidDataException($"类型不正确：{categoryId}");
                }

                return _mapper.Map<CategoryModel>(category);
            }

            return null;
        }

        /// <summary>
        /// 根据平台类型查询
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public async Task<PostListItemModel> GetByPlatformAsync(EnumPlatform platform, string dataId)
        {
            var _postRepository = _unitOfWork.GetRepository<IPostRepository>();
            var post = await _postRepository.GetFirstOrDefaultAsync(predicate:x=>x.PlatformInfo.Platform==platform&&x.PlatformInfo.PlatformId==dataId);
            if (post!=null)
            {
                return _mapper.Map<PostListItemModel>(post);
            }
            return null;
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryModel>> GetCategorysAsync()
        {
            var _postRepository = _unitOfWork.GetBaseRepository<Category>();
            var post = await _postRepository.GetAll(true).ToListAsync();
            if (post!=null)
            {
                return _mapper.Map<List<CategoryModel>>(post);
            }
            return null;
        }

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <returns></returns>
        public CategoryModel GetCategoryByNameAsync(string displayName)
        {
            var _postRepository = _unitOfWork.GetBaseRepository<Category>();
            var post = _postRepository.GetFirstOrDefault(predicate:x=>x.DisplayName==displayName,disableTracking:true);
            if (post!=null)
            {
                return _mapper.Map<CategoryModel>(post);
            }
            return null;
        }
    }
}