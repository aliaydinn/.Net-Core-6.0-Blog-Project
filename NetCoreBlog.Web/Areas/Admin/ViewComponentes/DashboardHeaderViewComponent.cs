using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Data.UnitOfWorks;
using NetCoreBlog.Entity.DTOs.Users;
using NetCoreBlog.Entity.Entities;

namespace NetCoreBlog.Web.Areas.Admin.ViewComponentes
{
    public class DashboardHeaderViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public DashboardHeaderViewComponent(UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var getLoggedInUser = await userManager.GetUserAsync(HttpContext.User);
            var user = await unitOfWork.GetRepository<AppUser>().GetByAsync(x => x.Id == getLoggedInUser.Id, x => x.Image);
            var map = mapper.Map<UserDto>(user);
            var role = string.Join("", await userManager.GetRolesAsync(getLoggedInUser));
            map.Role = role;
            map.Image.FileName = user.Image.FileName;
            return View(map);
        }


    }
}

