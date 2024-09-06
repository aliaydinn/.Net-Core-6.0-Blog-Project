using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetCoreBlog.Entity.DTOs.Auths
{
    public class SıgnInUserDto
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adını boş geçmeyiniz...")]
        [StringLength(15, ErrorMessage = "Lütfen kullanıcı adını 4 ile 15 karakter arasında giriniz...", MinimumLength = 2)]
        [Display(Name = "Kullanıcı Adı")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lütfen kullanıcı soyadını boş geçmeyiniz...")]
        [StringLength(15, ErrorMessage = "Lütfen kullanıcı soyadını 4 ile 15 karakter arasında giriniz...", MinimumLength = 2)]
        [Display(Name = "Kullanıcı Soyadı")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Lütfen emaili boş geçmeyiniz...")]
        [EmailAddress(ErrorMessage = "Lütfen email formatında bir değer belirtiniz...")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lütfen şifreyi boş geçmeyiniz...")]
        [DataType(DataType.Password, ErrorMessage = "Lütfen şifreyi tüm kuralları göz önüne alarak giriniz...")]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }
}
