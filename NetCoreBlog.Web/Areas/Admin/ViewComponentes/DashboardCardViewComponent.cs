using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Data.UnitOfWorks;
using NetCoreBlog.Entity.DTOs.Users;
using NetCoreBlog.Entity.Entities;

namespace NetCoreBlog.Web.Areas.Admin.ViewComponentes
{
    public class DashboardCardViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public DashboardCardViewComponent(UserManager<AppUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var map = mapper.Map<UserDto>(user);
            return View(map);
        }
    }
}
