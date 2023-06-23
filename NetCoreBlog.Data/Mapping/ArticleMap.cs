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
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasData(new Article
            {
                Id = Guid.NewGuid(),
                Title = "Asp.net Core Makale 1",
                Content = "Asp.net Core Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                ViewCount = 15,
                CategoryId=Guid.Parse("03077BCA-F0B5-4B5E-BFE6-DCDF367BC2C1"),
                ImageId=Guid.Parse("B231C514-A82E-4D0F-B540-78C564DECEF5"),
                UserId= Guid.Parse("5A831022-3C8E-491D-8786-CEA90460F2BF")

            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "Visual Studio Makale 1",
                Content = "Visual Studio It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy.",
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                ViewCount = 15,
                CategoryId=Guid.Parse("6229F224-1C1C-4957-9422-3F5C2749A475"),
                ImageId = Guid.Parse("DA7B2CC6-DB74-4EDE-B35B-B7FC8A77936D"),
                UserId= Guid.Parse("976C78C5-CF5E-4CA9-90A9-979E23A4243F")
            });
            
        }
    }
}
