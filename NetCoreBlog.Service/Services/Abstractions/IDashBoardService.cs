using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.Services.Abstractions
{
    public interface IDashBoardService
    {
        Task<List<int>> YearlyGetAllArticlesAsync();
        Task<int> GetTotalArticleAsync();
        Task<int> GetTotalCategoryAsync();
        Task<int> GetTotalUserAsync();
    }
}
