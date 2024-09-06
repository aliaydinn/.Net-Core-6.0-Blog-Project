using NetCoreBlog.Entity.DTOs.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Entity.ViewModels
{
    public class AuthViewModel
    {
        public UserLoginDto UserLoginDto { get; set; }
        public SıgnInUserDto SıgnInUserDto { get; set; }
    }
}
