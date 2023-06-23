using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Data.UnitOfWorks;
using NetCoreBlog.Entity.DTOs.Articles;
using NetCoreBlog.Entity.DTOs.Categories;
using NetCoreBlog.Entity.Entities;
using NetCoreBlog.Service.Extensions;
using NetCoreBlog.Service.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.Services.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }



        public async Task<List<CategoryDto>> GetAllCategoryNonIsDeleted()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);
            var map = mapper.Map<List<CategoryDto>>(categories);
            return map;

        }

        public async Task<List<Category>> GetAllCategoryHomeIndexNonIsDeleted()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);

            foreach (var category in categories)
            {
                var articleCount = await unitOfWork.GetRepository<Article>().CountAsync(a => a.CategoryId == category.Id && !a.IsDeleted);
                category.ArticleCount = articleCount;
            }

            await unitOfWork.SaveAsync();

            return categories.Take(14).ToList();
        }


        public async Task<List<CategoryDeleteDto>> GetAllCategoryDelete()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync(x => x.IsDeleted);
            var map=mapper.Map<List<CategoryDeleteDto>>(categories);
            return map;
        }

        public async Task CreateCategoryAsync(CategoryAddDto categoryAddDto)
        {
            var userEmail = httpContextAccessor.HttpContext.User.GetLoggedInEmail();
            Category category = new(categoryAddDto.Name, userEmail);
            await unitOfWork.GetRepository<Category>().AddAsync(category);
            await unitOfWork.SaveAsync();
        }

        public async Task<CategoryUpdateDto> GetCategoryByGuid(Guid categoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);
            var map=mapper.Map<CategoryUpdateDto>(category);
            return map;
        }

        public async Task<string> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var userEmail = httpContextAccessor.HttpContext.User.GetLoggedInEmail();
            var category = await unitOfWork.GetRepository<Category>().GetByAsync(x=>x.Id==categoryUpdateDto.Id);
            category.Name = categoryUpdateDto.Name;
            category.ModifiedBy = userEmail;
            category.ModifiendDate = DateTime.Now;
            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();
            return category.Name;

        }

        public async Task<string> SafeDeleteCategoryAsync(Guid categoryId)
        {
            var userEmail = httpContextAccessor.HttpContext.User.GetLoggedInEmail();
            var deleted=  await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);
            deleted.DeletedBy = userEmail;
            deleted.DeletedDate = DateTime.Now;
            deleted.IsDeleted = true;
            await unitOfWork.GetRepository<Category>().UpdateAsync(deleted);
            await unitOfWork.SaveAsync();
            return deleted.Name;
        }

        public async Task<string> UndoDeleteAsync(Guid categoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);
            category.IsDeleted= false;
            category.DeletedDate = null;
            category.DeletedBy = null;
            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();
            return category.Name;

        }
    }
}
