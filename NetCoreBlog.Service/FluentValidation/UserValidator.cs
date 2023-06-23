using FluentValidation;
using NetCoreBlog.Entity.DTOs.Users;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.FluentValidation
{
    public class UserValidator : AbstractValidator<AppUser>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName)
           .NotEmpty()
           .MinimumLength(3)
           .MaximumLength(15)
           .WithName("Kullanıcı Adı");

            RuleFor(x => x.LastName)
           .NotEmpty()
           .MinimumLength(3)
           .MaximumLength(15)
           .WithName("Kullanıcı Soyadı");

            RuleFor(x => x.PhoneNumber)
           .NotEmpty()
           .MinimumLength(11)
           .MaximumLength(11)
           .WithName("Telefon Numarası");


        }
    }
}
