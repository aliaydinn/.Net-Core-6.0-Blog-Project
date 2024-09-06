using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NetCoreBlog.Entity.DTOs.Auths;
using NetCoreBlog.Entity.Entities;
using NetCoreBlog.Entity.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Web;

namespace NetCoreBlog.Web.Areas.Admin.Controllers
{
    [Area(Consts.RoleConst.Admin)]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var viewModel = new AuthViewModel
            {
                SıgnInUserDto = new SıgnInUserDto(),
                UserLoginDto = new UserLoginDto()
            };
            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(userLoginDTO.Email);

                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, userLoginDTO.Password, userLoginDTO.RememberMe, false);

                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(user);
                        return RedirectToAction("Index", "Home", new { Area = "Admin" });
                    }
                    else if (result.IsLockedOut)
                    {
                        
                        var lockoutEnd = user.LockoutEnd;
                        if (lockoutEnd.HasValue)
                        {
                            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
                            DateTime lockoutEndLocal = TimeZoneInfo.ConvertTimeFromUtc(lockoutEnd.Value.UtcDateTime, timeZone);
                            ModelState.AddModelError("Locked", $"Hesabınız {lockoutEndLocal:HH:mm:ss} kadar kitlenmiştir. Lütfen sonra tekrar deneyin.");
                        }
                    }
                    else
                    {
                        // Başarısız giriş denemesi
                        await userManager.AccessFailedAsync(user);
                        int failcount = await userManager.GetAccessFailedCountAsync(user);

                        if (failcount >= 5)
                        {
                            await userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(5)));
                            
                            var lockoutEnd = user.LockoutEnd;

                            if (lockoutEnd.HasValue)
                            {
                                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
                                DateTime lockoutEndLocal = TimeZoneInfo.ConvertTimeFromUtc(lockoutEnd.Value.UtcDateTime, timeZone);
                                ModelState.AddModelError("Locked", $"Hesabınız {lockoutEndLocal:HH:mm:ss} kadar kitlenmiştir. Lütfen sonra tekrar deneyin.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "E-Mail adresiniz veya şifreniz hatalıdır.");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-Mail adresiniz veya şifreniz hatalıdır.");
                }

                var viewModel = new AuthViewModel
                {
                    SıgnInUserDto = new SıgnInUserDto(),
                    UserLoginDto = userLoginDTO
                };
                return View(viewModel);
            }
            else
            {
                var viewModel = new AuthViewModel
                {
                    SıgnInUserDto = new SıgnInUserDto(),
                    UserLoginDto = userLoginDTO
                };
                return View(viewModel);
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AccesDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PasswordReset()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordReset(ResetPasswordDto resetPasswordDto)
        {
            AppUser user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user is not null)
            {
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                string resetLink = Url.Action("UpdatePassword", "Auth", new { userId = user.Id, token = HttpUtility.UrlEncode(resetToken) }, Request.Scheme);

                MailMessage mail = new()
                {
                    IsBodyHtml = true,
                    To = { user.Email },
                    From = new MailAddress("dvlhuman64@gmail.com", "Şifre Sıfırlama", Encoding.UTF8),
                    Subject = "Şifre Sıfırlama Talebi",
                    Body = $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 20px;
        }}
        .header {{
            background: #007bff;
            color: #ffffff;
            padding: 10px 20px;
            border-radius: 8px 8px 0 0;
            text-align: center;
        }}
        .content {{
            padding: 20px;
            font-size: 16px;
            line-height: 1.5;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            font-size: 16px;
            color: #ffffff;
            background: #007bff;
            text-decoration: none;
            border-radius: 4px;
            text-align: center;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 14px;
            text-align: center;
            color: #777;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            Şifre Yenileme Talebi
        </div>
        <div class='content'>
            <p>Merhaba {user.UserName},</p>
            <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıyı kullanabilirsiniz:</p>
            <a href='{resetLink}' class='button'>Şifreyi Yenile</a>
            <p>Bu bağlantı birkaç saat içinde geçerliliğini yitirecektir.</p>
            <p>Herhangi bir sorun yaşarsanız bizimle iletişime geçebilirsiniz.</p>
            <p>Teşekkürler,<br>Ali Aydın</p>
        </div>
        
    </div>
</body>
</html>"
                };


                using (SmtpClient smp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smp.Credentials = new NetworkCredential("dvlhuman64@gmail.com", "pswp qwnq aprk ofri");
                    smp.EnableSsl = true;
                    smp.Send(mail);
                }

                ViewBag.State = true;
            }
            else
            {
                ViewBag.State = false;
            }

            return View();
        }


        [HttpGet("[action]/{userId}/{token}")]
        public IActionResult UpdatePassword(string userId, string token)
        {
            ViewData["UserId"] = userId;
            ViewData["Token"] = token;
            return View();
        }

        [HttpPost("[action]/{userId}/{token}")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updatePasswordDto, string userId, string token)
        {
            AppUser user = await userManager.FindByIdAsync(userId);

            if (updatePasswordDto.Password != updatePasswordDto.ConfirmPassword)
            {
                ModelState.AddModelError("", "Şifreler eşleşmiyor.");
                ViewData["UserId"] = userId;
                ViewData["Token"] = token;
                return View(updatePasswordDto);
            }

            IdentityResult result = await userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), updatePasswordDto.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla güncellenmiştir.";
                await userManager.UpdateSecurityStampAsync(user);
                return View();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewData["UserId"] = userId;
                ViewData["Token"] = token;
                return View(updatePasswordDto);
            }
        }


        [HttpGet]
        public IActionResult SignIn()
        {
            var viewModel = new AuthViewModel
            {
                SıgnInUserDto = new SıgnInUserDto(),
                UserLoginDto = new UserLoginDto()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SıgnInUserDto sıgnInUserDto)
        {
            if (ModelState.IsValid)
            {
                
                var appUser = new AppUser
                {
                    FirstName = sıgnInUserDto.FirstName,
                    LastName = sıgnInUserDto.LastName,
                    Email = sıgnInUserDto.Email,
                    UserName = sıgnInUserDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                IdentityResult result = await userManager.CreateAsync(appUser, sıgnInUserDto.Password);
                if (result.Succeeded)
                {
                    
                    return RedirectToAction("Login", "Auth", new { Area = "Admin" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("SıgnInErrors", error.Description);
                }

                ViewBag.ShowSignInErrors = true;
            }

            var viewModel = new AuthViewModel
            {
                SıgnInUserDto = sıgnInUserDto,
                UserLoginDto = new UserLoginDto()
            };
            return View("Login",viewModel);
        }

    }

}