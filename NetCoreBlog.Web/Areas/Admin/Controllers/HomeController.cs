using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Service.Services.Abstractions;
using Newtonsoft.Json;

namespace NetCoreBlog.Web.Areas.Admin.Controllers
{
    [Area(Consts.RoleConst.Admin)]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IArticleService articleService;
        private readonly IDashBoardService dashBoardService;

        public HomeController(IArticleService articleService, IDashBoardService dashBoardService)
        {
            this.articleService = articleService;
            this.dashBoardService = dashBoardService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> YearlyArticleCounts()
        {
            var counts = await dashBoardService.YearlyGetAllArticlesAsync();
            return Json(JsonConvert.SerializeObject(counts));
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalArticlesCount()
        {
            var totalarticleCounts = await dashBoardService.GetTotalArticleAsync();
            return Json(totalarticleCounts);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalCategoriesCount()
        {
            var totalcategoryCounts = await dashBoardService.GetTotalCategoryAsync();
            return Json(totalcategoryCounts);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalUsersCount()
        {
            var totaluserCounts = await dashBoardService.GetTotalUserAsync();
            return Json(totaluserCounts);
        }


    }
}
