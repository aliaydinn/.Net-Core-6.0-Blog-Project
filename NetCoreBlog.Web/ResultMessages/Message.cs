namespace NetCoreBlog.Web.ResultMessages
{
    public static class Message
    {
        public static class Article
        {
            public static string Add(string articleTitle)
            {
                return $"{articleTitle}         Başlıklı makale başarılı bir şekilde eklenmiştir.";
            }

            public static string Update(string articleTitle)
            {
                return $"{articleTitle}         Başlıklı makale başarılı bir şekilde güncellenmiştir.";
            }

            public static string Delete(string articleTitle)
            {
                return $"{articleTitle}         Başlıklı makale başarılı bir şekilde silinmiştir.";
            }
            public static string UndoDelete(string articleTitle)
            {
                return $"{articleTitle}         Başlıklı makale başarılı bir şekilde aktif hale getirilmiştir .";
            }
        }

        public static class Category
        {
            public static string Add(string categoryTitle)
            {
                return $"{categoryTitle}         Başlıklı kategori başarılı bir şekilde eklenmiştir.";
            }

            public static string Update(string categoryTitle)
            {
                return $"{categoryTitle}         Başlıklı kategori başarılı bir şekilde güncellenmiştir.";
            }

            public static string Delete(string categoryTitle)
            {
                return $"{categoryTitle}         Başlıklı kategori başarılı bir şekilde silinmiştir.";
            }

            public static string UndoDelete(string categoryTitle)
            {
                return $"{categoryTitle}         Başlıklı kategori başarılı bir şekilde aktif hale getirilmiştir.";
            }
        }

        public static class User
        {
            public static string Add(string userTitle)
            {
                return $"{userTitle}         adlı kullanıcı başarılı bir şekilde eklenmiştir.";
            }

            public static string Update(string userTitle)
            {
                return $"{userTitle}          adlı kullanıcı başarılı bir şekilde güncellenmiştir.";
            }

            public static string Delete(string userTitle)
            {
                return $"{userTitle}         adlı kullanıcı başarılı bir şekilde silinmiştir.";
            }
        }
    }
}
