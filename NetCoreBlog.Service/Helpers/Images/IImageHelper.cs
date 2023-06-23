using Microsoft.AspNetCore.Http;
using NetCoreBlog.Entity.DTOs.Images;
using NetCoreBlog.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Service.Helpers.Images
{
    public interface IImageHelper
    {
        Task<ImageUploadedDto> Uploaded(string name, IFormFile imageFile,ImageType ımageType ,string folderName = null);
        void Delete(string imageName);
    }
}
