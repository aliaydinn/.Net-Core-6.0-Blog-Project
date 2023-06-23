using Microsoft.AspNetCore.Http;
using NetCoreBlog.Entity.DTOs.Categories;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Entity.DTOs.Articles
{
    public  class ArticleAddDto
    {
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
        public string Content { get; set; }
        public IFormFile Photo { get; set; }
        public Image Image { get; set; }
        public IList<CategoryDto> Categories { get; set; }
        public int MinRead { get; set; }
    }
}
