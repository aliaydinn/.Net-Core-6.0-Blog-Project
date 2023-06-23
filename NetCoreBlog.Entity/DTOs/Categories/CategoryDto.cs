﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Entity.DTOs.Categories
{
    public class CategoryDto
    {

        public CategoryDto(string name, string createdby)
        {
            Name = name;
            CreatedBy = createdby;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ArticleCount { get; set; }

    }
}
