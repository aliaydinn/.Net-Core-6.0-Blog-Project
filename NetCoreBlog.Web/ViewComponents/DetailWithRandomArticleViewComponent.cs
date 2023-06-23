using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Service.Services.Abstractions;

namespace NetCoreBlog.Web.ViewComponents
{
    public class DetailWithRandomArticleViewComponent : ViewComponent
    {
        private readonly IArticleService articleService;

        public DetailWithRandomArticleViewComponent(IArticleService articleService)
        {
            this.articleService = articleService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var articles = await articleService.ListRandomGetArticleAsync();
            return View(articles);
        }
    }
}
