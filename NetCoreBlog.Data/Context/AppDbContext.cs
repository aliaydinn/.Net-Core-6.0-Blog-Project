using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Data.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ArticleVisitor> ArticleVisitors { get; set; }
        public DbSet<Visitor> Visitors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Article>().HasData(new Article
            //{
            //    Id=Guid.Parse("2"),
            //    Content="blablabla"

            //});
            base.OnModelCreating(builder);  // Migration oluşturuken sorun çıkarmaması için yazıyoruz.
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          

        }

    }
}
