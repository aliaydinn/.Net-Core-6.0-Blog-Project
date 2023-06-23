using AutoMapper;
using NetCoreBlog.Entity.DTOs.Articles;
using NetCoreBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.AutoMapper.Articles
{
    public class ArticleProfile : Profile
    {

        public ArticleProfile()
        {
            CreateMap<ArticleDto, Article>().ReverseMap();
            CreateMap<ArticleUpdateDto, Article>().ReverseMap();
            CreateMap<ArticleDto, ArticleUpdateDto>().ReverseMap();
            CreateMap<ArticleUpdateDto,ArticleAddDto>().ReverseMap();
            CreateMap<ArticleAddDto,Article>().ReverseMap();
            CreateMap<ArticleUndoDelete,Article>().ReverseMap();
        }
    }
}
