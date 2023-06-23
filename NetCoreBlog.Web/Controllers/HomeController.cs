using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Data.UnitOfWorks;
using NetCoreBlog.Entity.Entities;
using NetCoreBlog.Service.Services.Abstractions;
using NetCoreBlog.Web.Models;
using NToastNotify;
using System.Diagnostics;

namespace NetCoreBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService articleService;
        private readonly IToastNotification toast;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger,IArticleService articleService,IToastNotification toast,IHttpContextAccessor httpContextAccessor,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.articleService = articleService;
            this.toast = toast;
            this.httpContextAccessor = httpContextAccessor;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            var articles = await articleService.GetAllPagingAsync(categoryId,currentPage,pageSize,isAscending);
            return View(articles);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid articleId)
        {
            var allArticleVisitors=await unitOfWork.GetRepository<ArticleVisitor>().GetAllAsync(null,x=>x.Visitor,y=>y.Article);

            var ıpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            var article = await unitOfWork.GetRepository<Article>().GetByAsync(x => x.Id == articleId);
            var visitor = await unitOfWork.GetRepository<Visitor>().GetByAsync(x => x.IpAddress == ıpAddress);
            var result = await articleService.GetArticleWithUserNoneDeletedAsync(articleId);
            var articleVisitor=new ArticleVisitor(article.Id,visitor.Id);

            if (allArticleVisitors.Any(x=>x.ArticleId==articleVisitor.ArticleId && x.VisitorId==articleVisitor.VisitorId))
            {
                return View(result);
            }
            else
            {
                await unitOfWork.GetRepository<ArticleVisitor>().AddAsync(articleVisitor);
                article.ViewCount += 1;
                await unitOfWork.GetRepository<Article>().UpdateAsync(article);
                await unitOfWork.SaveAsync();
            }
            return View(result);


        }
        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            var search = await articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);
            return View(search);
        }


    }
}