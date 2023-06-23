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
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(new Category
            {
                Id= Guid.Parse("03077BCA-F0B5-4B5E-BFE6-DCDF367BC2C1"),
                Name ="Asp.net Core",
                CreatedBy="Admin Test",
                CreatedDate=DateTime.Now,
                IsDeleted=false

            },
            new Category
            {

                Id = Guid.Parse("6229F224-1C1C-4957-9422-3F5C2749A475"),
                Name = "Visual Studio 2022",
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false
            });
        }
    }
}
