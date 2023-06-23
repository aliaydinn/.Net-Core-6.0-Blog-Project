using Microsoft.AspNetCore.Hosting;
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
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment env;
        private readonly string wwwroot;
        private const string imgFolder = "images";
        private const string articleİmagesFolder = "article-images";
        private const string userİmagesFolder = "user-images";

        public ImageHelper(IWebHostEnvironment env)
        {
            this.env = env;
            wwwroot = env.WebRootPath;
        }

        private string ReplaceInvalidChars(string fileName)
        {
            return fileName.Replace("İ", "I")
                 .Replace("ı", "i")
                 .Replace("Ğ", "G")
                 .Replace("ğ", "g")
                 .Replace("Ü", "U")
                 .Replace("ü", "u")
                 .Replace("ş", "s")
                 .Replace("Ş", "S")
                 .Replace("Ö", "O")
                 .Replace("ö", "o")
                 .Replace("Ç", "C")
                 .Replace("ç", "c")
                 .Replace("é", "")
                 .Replace("!", "")
                 .Replace("'", "")
                 .Replace("^", "")
                 .Replace("+", "")
                 .Replace("%", "")
                 .Replace("/", "")
                 .Replace("(", "")
                 .Replace(")", "")
                 .Replace("=", "")
                 .Replace("?", "")
                 .Replace("_", "")
                 .Replace("*", "")
                 .Replace("æ", "")
                 .Replace("ß", "")
                 .Replace("@", "")
                 .Replace("€", "")
                 .Replace("<", "")
                 .Replace(">", "")
                 .Replace("#", "")
                 .Replace("$", "")
                 .Replace("½", "")
                 .Replace("{", "")
                 .Replace("[", "")
                 .Replace("]", "")
                 .Replace("}", "")
                 .Replace(@"\", "")
                 .Replace("|", "")
                 .Replace("~", "")
                 .Replace("¨", "")
                 .Replace(",", "")
                 .Replace(";", "")
                 .Replace("`", "")
                 .Replace(".", "")
                 .Replace(":", "")
                 .Replace(" ", "");
        }

        public async Task<ImageUploadedDto> Uploaded(string name, IFormFile imageFile,ImageType ımageType, string folderName = null)
        {
            //folderName ??= ımageType == ImageType.User ? userİmagesFolder : articleİmagesFolder;

            if (ımageType == ImageType.User)
            {
                folderName = userİmagesFolder;
            }
            else
            {
                folderName = articleİmagesFolder;
            }

            if (!Directory.Exists($"{wwwroot}/{imgFolder}/{folderName}"))
                Directory.CreateDirectory($"{wwwroot}/{imgFolder}/{folderName}");
                

            string oldFileName=Path.GetFileNameWithoutExtension(imageFile.FileName);
            string fileExtension=Path.GetExtension(imageFile.FileName);

            name=ReplaceInvalidChars(name);
            var date = DateTime.Now;
            string newFileName = $"{name}_{date.Millisecond}{fileExtension}";
            var path = Path.Combine($"{wwwroot}/{imgFolder}/{folderName}", newFileName);

            await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
            await imageFile.CopyToAsync(stream);
            await stream.FlushAsync();

            var message = ımageType == ImageType.User ? $"{newFileName} isimli kullancı resmi başaralı bir şekilde eklenmiştir  " :
                $"{newFileName} başlıklı makale resmi başaralı bir şekilde eklenmiştir";

            return new ImageUploadedDto
            {
                FullName = $"{folderName}/{newFileName}"
            };
        }

        public void Delete(string imageName)
        {
            var filetodeleted = Path.Combine($"{wwwroot}/{imgFolder}/{imageName}");
            if (File.Exists(filetodeleted))
                File.Delete(filetodeleted);
        }
    }
}
