using NetCoreBlog.Entity.DTOs.Categories;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoryNonIsDeleted();
        Task<List<CategoryDeleteDto>> GetAllCategoryDelete();
        Task<List<Category>> GetAllCategoryHomeIndexNonIsDeleted();
        Task CreateCategoryAsync(CategoryAddDto categoryAddDto);
        Task<CategoryUpdateDto> GetCategoryByGuid(Guid categoryId);
        Task<string> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
        Task<string> SafeDeleteCategoryAsync(Guid categoryId);
        Task<string> UndoDeleteAsync(Guid categoryId);
    }
}
