using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Service.Services.Abstractions;

namespace NetCoreBlog.Web.ViewComponents
{
    public class MostReadArticleViewComponent:ViewComponent
    {
        private readonly IArticleService articleService;

        public MostReadArticleViewComponent(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var article=await articleService.MostReadArticleAsync();
            return View(article);
        }
    }
}
