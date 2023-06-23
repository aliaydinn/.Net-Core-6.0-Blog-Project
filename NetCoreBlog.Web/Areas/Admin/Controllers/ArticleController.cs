using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Entity.DTOs.Articles;
using NetCoreBlog.Entity.Entities;
using NetCoreBlog.Service.Extensions;
using NetCoreBlog.Service.Services.Abstractions;
using NetCoreBlog.Web.Consts;
using NetCoreBlog.Web.ResultMessages;
using NToastNotify;

namespace NetCoreBlog.Web.Areas.Admin.Controllers
{
    [Area(RoleConst.Admin)]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Article> validator;
        private readonly IToastNotification toast;

        public ArticleController(IArticleService articleService,ICategoryService categoryService,IMapper mapper,IValidator<Article> validator,IToastNotification toast)
        {
            this.articleService = articleService;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;
        }
        [Authorize(Roles =$"{RoleConst.Superadmin},{RoleConst.Admin},{RoleConst.User}")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await articleService.GetAllArticleWithCategoryNoneDeletedAsync();
            return View(articles);
        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
		[HttpGet]
        public async Task<IActionResult> DeletedArticles()
        {
            var article = await articleService.GetAllArticleWithCategoryDeletedAsync();
            return View(article);
        }
        [Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
		[HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await categoryService.GetAllCategoryNonIsDeleted();
            var dto = new ArticleAddDto
            {
                Categories = categories
            };
            return View(dto);
        }
        [Authorize(Roles =$"{RoleConst.Superadmin},{RoleConst.Admin}")]
        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddDto articleAddDto)
        {

            var map = mapper.Map<Article>(articleAddDto);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await articleService.CreateArticleAsync(articleAddDto);
                toast.AddSuccessToastMessage(Message.Article.Add(articleAddDto.Title), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
               
            }
            else
            {
                result.AddToModelState(this.ModelState);
               
            }
            var categories = await categoryService.GetAllCategoryNonIsDeleted();
            return View(new ArticleAddDto { Categories = categories });

        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
		[HttpGet]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await articleService.GetArticleWithCategoryNoneDeletedAsync(articleId);
            var categories = await categoryService.GetAllCategoryNonIsDeleted();
            var articleUpdateDto=mapper.Map<ArticleUpdateDto>(article);
            articleUpdateDto.Categories = categories;
            return View(articleUpdateDto);

            
        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
		[HttpPost]
        public async Task<IActionResult> Update(ArticleUpdateDto articleUpdateDto)
        {

            var map = mapper.Map<Article>(articleUpdateDto);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var title= await articleService.UpdateArticleAsync(articleUpdateDto);
                toast.AddWarningToastMessage(Message.Article.Update(title), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState); 
            }
            var categories = await categoryService.GetAllCategoryNonIsDeleted();
            articleUpdateDto.Categories = categories;
            return View(articleUpdateDto);

        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
        [HttpGet]
		public async Task<IActionResult> Delete(Guid articleId)
        {
            var title= await articleService.SafeDeleteArticleAsync(articleId);
            toast.AddErrorToastMessage(Message.Article.Delete(title), new ToastrOptions { Title = "Başarılı" });
            return RedirectToAction("Index", "Article", new { Area = "Admin" });

        }
		[Authorize(Roles = $"{RoleConst.Superadmin}")]
        [HttpGet]
		public async Task<IActionResult> UndoDelete(Guid articleId)
        {
            var title = await articleService.UndoDeleteArticleAsync(articleId);
            toast.AddErrorToastMessage(Message.Article.UndoDelete(title), new ToastrOptions { Title = "Başarılı" });
            return RedirectToAction("Index", "Article", new { Area = "Admin" });

        }

    }
}
