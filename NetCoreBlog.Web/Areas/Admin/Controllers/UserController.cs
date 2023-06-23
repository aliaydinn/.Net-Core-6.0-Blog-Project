using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Data.UnitOfWorks;
using NetCoreBlog.Entity.DTOs.Articles;
using NetCoreBlog.Entity.DTOs.Users;
using NetCoreBlog.Entity.Entities;
using NetCoreBlog.Entity.Enums;
using NetCoreBlog.Service.Extensions;
using NetCoreBlog.Service.Helpers.Images;
using NetCoreBlog.Service.Services.Abstractions;
using NetCoreBlog.Web.Consts;
using NetCoreBlog.Web.ResultMessages;
using NToastNotify;

namespace NetCoreBlog.Web.Areas.Admin.Controllers
{
    [Area(Consts.RoleConst.Admin)]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IValidator<AppUser> validator;
        private readonly IToastNotification toast;


        public UserController(IUserService userService, IMapper mapper, IValidator<AppUser> validator, IToastNotification toast)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;

        }
        [Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin},{RoleConst.User}")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await userService.GetAllUsersWithRolesAsync();
            return View(users);
        }
        [Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await userService.GetAllRolesAsync();
            var dto = new UserAddDto
            {
                Roles = roles
            };
            return View(dto);
        }
        [Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            var map = mapper.Map<AppUser>(userAddDto);
            var validate = validator.Validate(map);
            var roles = await userService.GetAllRolesAsync();
            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(userAddDto);
                if (result.Succeeded)
                {
                    toast.AddSuccessToastMessage(Message.User.Add(userAddDto.Email), new ToastrOptions { Title = "İşlem Başarılı" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    result.AddToIdentityModelState(this.ModelState);
                    validate.AddToModelState(this.ModelState);
                    return View(new UserAddDto { Roles = roles });
                }
            }
            else
            {
                return View(new UserAddDto { Roles = roles });
            }
        }
        [Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
        [HttpGet]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await userService.GetAppUserByIdAsync(userId);
            var roles = await userService.GetAllRolesAsync();
            var map = mapper.Map<UserUpdateDto>(user);

            map.Roles = roles;
            return View(map);
        }
        [Authorize(Roles = $"{RoleConst.Superadmin},{RoleConst.Admin}")]
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            var user = await userService.GetAppUserByIdAsync(userUpdateDto.Id);
            if (user != null)
            {
                var roles = await userService.GetAllRolesAsync();
                if (ModelState.IsValid)
                {
                    var map = mapper.Map(userUpdateDto, user);
                    var validate = validator.Validate(map);
                    if (validate.IsValid)
                    {
                        map.UserName = userUpdateDto.Email;
                        map.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await userService.UpdateUserAsync(userUpdateDto);
                        if (result.Succeeded)
                        {

                            toast.AddWarningToastMessage(Message.User.Update(userUpdateDto.Email), new ToastrOptions { Title = "İşlem Başarılı" });
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                        else
                        {
                            result.AddToIdentityModelState(this.ModelState);
                            return View(new UserUpdateDto { Roles = roles });
                        }
                    }
                    else
                    {
                        validate.AddToModelState(this.ModelState);
                        return View(new UserUpdateDto { Roles = roles });
                    }

                }

            }
            return NotFound();
        }
        [Authorize(Roles = $"{RoleConst.Superadmin}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await userService.DeleteUserAsync(userId);
            if (result.IdentityResult.Succeeded)
            {
                toast.AddErrorToastMessage(Message.User.Delete(result.email), new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.IdentityResult.AddToIdentityModelState(this.ModelState);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            toast.AddWarningToastMessage("Güncelleme işlemi yapmak istiyorsanız (Mevcut şifre) alanı boş geçilmemelidir !  ", new ToastrOptions { Title = "Uyarı ! " });
            var getProfile = await userService.GetUserProfileAsync();
            return View(getProfile);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileDto userProfileDto)
        {
           
            if (ModelState.IsValid)
            {
                var result=await userService.ProfileUpdateAsync(userProfileDto);
                if (result)
                {
                    toast.AddSuccessToastMessage("Profil bilgileriniz başarıyla güncellenmiştir .  ", new ToastrOptions { Title = "İşlem Başarılı " });
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                else
                {
                    var profile = await userService.GetUserProfileAsync();
                    toast.AddErrorToastMessage("Profil bilgileriniz güncellenirken bir hata oluştu .  ", new ToastrOptions { Title = "İşlem Başarısız ! " });
                    return View(profile);

                }
            }
            else
            {
                toast.AddErrorToastMessage("Profil bilgileriniz güncellenirken bir hata oluştu .  ", new ToastrOptions { Title = "İşlem Başarısız ! " });
                return NotFound();
            }


        }

    }

}
