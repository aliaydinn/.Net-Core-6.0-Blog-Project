using NetCoreBlog.Data.UnitOfWorks;
using NetCoreBlog.Entity.Entities;
using NetCoreBlog.Service.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.Services.Concretes
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IUnitOfWork unitOfWork;

        public DashBoardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<int>> YearlyGetAllArticlesAsync()
        {
            var articles=await unitOfWork.GetRepository<Article>().GetAllAsync(x=>!x.IsDeleted);

            var startDate = DateTime.Now.Date;
            startDate = new(2022, 1, 1);

            List<int> datas = new();

            for (int i = 1; i <= 12; i++)
            {
                var startedDate=new DateTime(2022, i, 1);
                var endedDate=startedDate.AddMonths(1);
                var data = articles.Where(x => x.CreatedDate >= startedDate && x.CreatedDate < endedDate).Count();
                datas.Add(data);
            }
            return datas;
        }

        public async Task<int> GetTotalArticleAsync()
        {
            var articleCounts = await unitOfWork.GetRepository<Article>().CountAsync();
            return articleCounts;
        }

        public async Task<int> GetTotalCategoryAsync()
        {
            var categoryCounts = await unitOfWork.GetRepository<Category>().CountAsync();
            return categoryCounts;
        }

        public async Task<int> GetTotalUserAsync()
        {
            var userCounts = await unitOfWork.GetRepository<AppUser>().CountAsync();
            return userCounts;
        }


    }
}
