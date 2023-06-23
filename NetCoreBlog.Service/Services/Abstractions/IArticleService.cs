
using NetCoreBlog.Entity.DTOs.Articles;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.Services.Abstractions
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetAllArticleWithCategoryNoneDeletedAsync();
        Task<List<ArticleUndoDelete>> GetAllArticleWithCategoryDeletedAsync();
        Task<ArticleDto> GetArticleWithCategoryNoneDeletedAsync(Guid articleId);
        Task CreateArticleAsync(ArticleAddDto articleAddDto);
        Task<string> UpdateArticleAsync(ArticleUpdateDto articleUpdateDto);
        Task<string> SafeDeleteArticleAsync(Guid articleId);
        Task<string> UndoDeleteArticleAsync(Guid articleId);
        Task<ArticleDto> GetArticleWithUserNoneDeletedAsync(Guid articleId);
        Task<ArticleListDto> GetAllPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<ArticleListDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<List<Article>> ListRandomGetArticleAsync();
        Task<List<Article>> MostReadArticleAsync();
    }
}
