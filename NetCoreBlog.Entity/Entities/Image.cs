using NetCoreBlog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Entity.Entities
{
    public class Image : EntityBase
    {
        public Image()
        {

        }
        public Image(string filename,string filetype,string createdby)
        {
            FileName = filename;
            FileType = filetype;
            CreatedBy = createdby;
        }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<AppUser> Users { get; set; }
    }
}
