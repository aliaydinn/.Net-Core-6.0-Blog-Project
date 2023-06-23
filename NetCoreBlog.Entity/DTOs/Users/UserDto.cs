using Microsoft.AspNetCore.Http;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Entity.DTOs.Users
{
    public class UserDto
    {
       
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public int  AccessFailedCount { get; set; }
        public Image? Image { get; set; }

    }
}
