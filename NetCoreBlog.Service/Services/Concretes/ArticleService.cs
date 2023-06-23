using AutoMapper;
using NetCoreBlog.Entity.DTOs.Articles;
using NetCoreBlog.Data.UnitOfWorks;
using NetCoreBlog.Entity.Entities;
using NetCoreBlog.Service.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using NetCoreBlog.Service.Extensions;
using System.Runtime.InteropServices;
using NetCoreBlog.Service.Helpers.Images;
using NetCoreBlog.Entity.Enums;
using System.Runtime.CompilerServices;

namespace NetCoreBlog.Service.Services.Concretes
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IImageHelper ımageHelper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper ımageHelper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.ımageHelper = ımageHelper;
        }


        public async Task<ArticleListDto> GetAllPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var articles = categoryId == null
                ? await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted, x => x.Category, a => a.Image, y => y.User.Image) :
                 await unitOfWork.GetRepository<Article>().GetAllAsync(x => x.CategoryId == categoryId && !x.IsDeleted, x => x.Category, a => a.Image, y => y.User.Image);
            var sortedArticles = isAscending
           ? articles.OrderBy(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
           : articles.OrderByDescending(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new ArticleListDto
            {
                Articles = sortedArticles,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }


        public async Task<ArticleListDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted && (x.Title.Contains(keyword) || x.Content.Contains(keyword) || x.Category.Name.Contains(keyword)), x => x.Category, a => a.Image, y => y.User);


            var sortedArticles = isAscending
           ? articles.OrderBy(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
           : articles.OrderByDescending(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new ArticleListDto
            {
                Articles = sortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }

        public async Task<List<Article>> ListRandomGetArticleAsync()
        {
            Random random = new Random();
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted,y=>y.Image,z=>z.User.Image,c=>c.Category);
            var list = articles.OrderBy(x=>random.Next()).Take(2);
            return new List<Article>(list);
        }

        public async Task<List<Article>> MostReadArticleAsync()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted, y => y.Image, b=>b.User.Image,z=>z.Category);
            

            var mostViewedArticle = articles
                .OrderByDescending(x => x.ViewCount)
                .Take(2);

            return new List<Article>(mostViewedArticle);
        }
        public async Task<List<ArticleDto>> GetAllArticleWithCategoryNoneDeletedAsync()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted, x => x.Category);
            var map = mapper.Map<List<ArticleDto>>(articles);
            return map;
        }

        public async Task<List<ArticleUndoDelete>> GetAllArticleWithCategoryDeletedAsync()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => x.IsDeleted, x => x.Category);
            var map = mapper.Map<List<ArticleUndoDelete>>(articles);
            return map;
        }



        public async Task CreateArticleAsync(ArticleAddDto articleAddDto)
        {

            var userId = httpContextAccessor.HttpContext.User.GetLoggedInUserId();
            var userEmail = httpContextAccessor.HttpContext.User.GetLoggedInEmail();

            var imageupload = await ımageHelper.Uploaded(articleAddDto.Title, articleAddDto.Photo, ImageType.Post);
            Image image = new(imageupload.FullName, articleAddDto.Photo.ContentType, userEmail);
            await unitOfWork.GetRepository<Image>().AddAsync(image);

            var article = new Article(articleAddDto.Title, articleAddDto.Content, userId, userEmail, articleAddDto.CategoryId, image.Id,articleAddDto.MinRead);

            await unitOfWork.GetRepository<Article>().AddAsync(article);
            await unitOfWork.SaveAsync();
        }


        public async Task<ArticleDto> GetArticleWithCategoryNoneDeletedAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByAsync(x => !x.IsDeleted && x.Id == articleId, x => x.Category, x => x.Image, y => y.User);
            var map = mapper.Map<ArticleDto>(article);
            return map;
        }


        public async Task<ArticleDto> GetArticleWithUserNoneDeletedAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByAsync(x => !x.IsDeleted && x.Id == articleId, x => x.Category, x => x.Image, y => y.User.Image);
            var map = mapper.Map<ArticleDto>(article);
            return map;
        }



        public async Task<string> UpdateArticleAsync(ArticleUpdateDto articleUpdateDto)
        {
            var userEmail = httpContextAccessor.HttpContext.User.GetLoggedInEmail();
            var article = await unitOfWork.GetRepository<Article>().GetByAsync(x => !x.IsDeleted && x.Id == articleUpdateDto.Id, x => x.Category, y => y.Image);

            if (articleUpdateDto.Photo != null)
            {
                ımageHelper.Delete(article.Image.FileName);
                var uploadedImage = await ımageHelper.Uploaded(articleUpdateDto.Title, articleUpdateDto.Photo, ImageType.Post);
                Image image = new(uploadedImage.FullName, articleUpdateDto.Photo.ContentType, userEmail);
                await unitOfWork.GetRepository<Image>().AddAsync(image);
                article.ImageId = image.Id;

            }


            article.Title = articleUpdateDto.Title;
            article.CategoryId = articleUpdateDto.CategoryId;
            article.Content = articleUpdateDto.Content;
            article.ModifiendDate = DateTime.Now;
            article.ModifiedBy = userEmail;
            article.MinRead = articleUpdateDto.MinRead;
            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }

        public async Task<string> SafeDeleteArticleAsync(Guid articleId)
        {
            var userEmail = httpContextAccessor.HttpContext.User.GetLoggedInEmail();
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            article.IsDeleted = true;
            article.DeletedDate = DateTime.Now;
            article.DeletedBy = userEmail;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;

        }

        public async Task<string> UndoDeleteArticleAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            article.IsDeleted = false;
            article.DeletedDate = null;
            article.DeletedBy = null;
            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
            return article.Title;
        }


    }
}
