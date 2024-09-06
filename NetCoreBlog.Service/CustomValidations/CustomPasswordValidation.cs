using Microsoft.AspNetCore.Identity;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.CustomValidations
{
    public class CustomPasswordValidation : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> errors = new();
            if (password.Length < 5)
                errors.Add(new IdentityError { Code = "PasswordLength", Description = "Şifre 5 karakterden küçük olamaz ." });
            if (password.ToLower().Contains(user.FirstName.ToLower()))
                errors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = "Şifreniz kullanıcı adı içermemelidir ." });
            if (password.ToLower().Contains(user.LastName.ToLower()))
                errors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = "Şifreniz kullanıcı adı içermemelidir ." });
            if (!errors.Any())
                return Task.FromResult(IdentityResult.Success);
            else
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));

        }
    }
}
