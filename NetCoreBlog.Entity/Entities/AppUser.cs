using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NetCoreBlog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>,IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid ImageId { get; set; } = Guid.Parse("fb63663c-3426-42a1-aeba-9d078e6bcbbc");
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
