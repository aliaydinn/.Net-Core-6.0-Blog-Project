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
    public class ImageMap : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasData(new Image
            {
                Id = Guid.Parse("B231C514-A82E-4D0F-B540-78C564DECEF5"),
                FileName = "Images/testimage",
                FileType = "jpg",
                IsDeleted = false,
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,

            },
            new Image
            {
                Id = Guid.Parse("DA7B2CC6-DB74-4EDE-B35B-B7FC8A77936D"),
                FileName = "Images/vstest",
                FileType = "png",
                IsDeleted = false,
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,

            });
        }
    }
}
