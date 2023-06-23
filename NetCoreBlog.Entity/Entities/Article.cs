 using NetCoreBlog.Core.Entities;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Entity.Entities
{
    public class Article : EntityBase 
    {
        public Article()
        {

        }
        public Article(string title,string content,Guid userıd,string email,Guid categoryıd,Guid imageıd,int minRead)
        {
            Title = title;
            Content = content;
            UserId = userıd;
            CategoryId = categoryıd;
            ImageId = imageıd;
            CreatedBy = email;
            MinRead = minRead;
        }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; } = 0;
        public int MinRead { get; set; } 

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid? ImageId { get; set; }
        public Image Image { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<ArticleVisitor> ArticleVisitors { get; set; }





    }
   
}
