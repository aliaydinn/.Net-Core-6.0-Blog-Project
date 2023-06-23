using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Service.Services.Abstractions;

namespace NetCoreBlog.Web.ViewComponents
{
    public class HomeIndexCategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public HomeIndexCategoryViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories=await categoryService.GetAllCategoryHomeIndexNonIsDeleted();
            
            return View(categories);
        }
    }
}
