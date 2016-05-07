using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IArticleService 
    {
        IQueryable<Article> getArticleByUserId(long userId);

        IQueryable<Article> getArticleById(long articleId);
    }
}
