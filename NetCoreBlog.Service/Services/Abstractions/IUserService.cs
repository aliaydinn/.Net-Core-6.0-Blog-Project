using Microsoft.AspNetCore.Identity;
using NetCoreBlog.Entity.DTOs.Articles;
using NetCoreBlog.Entity.DTOs.Users;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.Services.Abstractions
{
    public  interface IUserService
    {
        Task<List<UserDto>> GetAllUsersWithRolesAsync();
        Task<List<AppRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateUserAsync(UserAddDto userAddDto);
        Task<IdentityResult> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<(IdentityResult IdentityResult, string? email)> DeleteUserAsync(Guid userId);
        Task<AppUser> GetAppUserByIdAsync(Guid userId);
        Task<string> GetUserRoleAsync(AppUser user);
        Task<UserProfileDto> GetUserProfileAsync();
        Task<bool> ProfileUpdateAsync(UserProfileDto userProfileDto);


    }
}
