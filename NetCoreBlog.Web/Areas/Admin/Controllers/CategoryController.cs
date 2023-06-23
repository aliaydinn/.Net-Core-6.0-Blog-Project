using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using NetCoreBlog.Entity.DTOs.Articles;
using NetCoreBlog.Entity.DTOs.Categories;
using NetCoreBlog.Entity.Entities;
using NetCoreBlog.Service.Extensions;
using NetCoreBlog.Service.Services.Abstractions;
using NetCoreBlog.Service.Services.Concretes;
using NetCoreBlog.Web.Consts;
using NetCoreBlog.Web.ResultMessages;
using NToastNotify;
using System.Data;

namespace NetCoreBlog.Web.Areas.Admin.Controllers
{
	[Area(Consts.RoleConst.Admin)]
	public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Category> validator;
        private readonly IToastNotification toast;

        public CategoryController(ICategoryService categoryService,IMapper mapper,IValidator<Category> validator,IToastNotification toast)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
			this.validator = validator;
            this.toast = toast;
        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin},{RoleConst.User}")]
		[HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories=await categoryService.GetAllCategoryNonIsDeleted();
            return View(categories);
        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin},{RoleConst.User}")]
		[HttpGet]
        public async Task<IActionResult> DeletedCategories()
        {
            var categories = await categoryService.GetAllCategoryDelete();
            return View(categories);
        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
		[HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
		[HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            var map=mapper.Map<Category>(categoryAddDto);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await categoryService.CreateCategoryAsync(categoryAddDto);
                toast.AddSuccessToastMessage(Message.Category.Add(categoryAddDto.Name), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });

            }
            else
            {
                result.AddToModelState(this.ModelState);

            }
            return View();
        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
		[HttpGet]
        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category=await categoryService.GetCategoryByGuid(categoryId);
            return View(category);
            
            
        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
		[HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            var map = mapper.Map<Category>(categoryUpdateDto);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var title = await categoryService.UpdateCategoryAsync(categoryUpdateDto);
                toast.AddWarningToastMessage(Message.Category.Update(title), new ToastrOptions { Title = "İşlem Başarılı !" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
                return View();
            }

        }
		[Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
        [HttpGet]
		public async Task<IActionResult> Delete(Guid categoryId)
        {
            var title= await categoryService.SafeDeleteCategoryAsync(categoryId);
            toast.AddErrorToastMessage(Message.Category.Delete(title), new ToastrOptions { Title = "İşlem Başarılı !" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });

        }
		[Authorize(Roles = $"{RoleConst.Superadmin}")]
		public async Task<IActionResult> UndoDelete(Guid categoryId)
        {
            var deleted = await categoryService.UndoDeleteAsync(categoryId);
            toast.AddAlertToastMessage(Message.Category.UndoDelete(deleted), new ToastrOptions { Title = " İşlem Başarılı " });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });

        }
    }
}
