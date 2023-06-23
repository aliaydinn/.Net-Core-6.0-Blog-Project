using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Data.Mapping
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            // Primary key
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            builder.HasData(new AppUserRole
            {
                UserId = Guid.Parse("5A831022-3C8E-491D-8786-CEA90460F2BF"),
                RoleId = Guid.Parse("FA04AAE7-A492-406D-B6FF-57C511D3D516")
            },
            new AppUserRole
            {
                UserId = Guid.Parse("976C78C5-CF5E-4CA9-90A9-979E23A4243F"),
                RoleId = Guid.Parse("B0FCACF6-9C50-43FB-AB56-85502C609E40")
            });
        }
    }
}
