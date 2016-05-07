using MyUniversity.Models;
using MyUniversity.Models.Repositories.Interface;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services
{
    public class ArticleService : IArticleService
    {
        public IArticleRepository _articleRepository { get; private set; }

        public ArticleService(IArticleRepository articleRepository)
        {
            this._articleRepository = articleRepository;
        }

        #region 条件检索
        public IQueryable<Article> getArticleByUserId(long userId)
        {
            return _articleRepository.Get(o => o.userId == userId, 1, 10, o => o.articleTime, false).AsQueryable();
        }

        public IQueryable<Article> getArticleById(long articleId)
        {
            return _articleRepository.Get(o => o.articleId == articleId).AsQueryable();
        }
        #endregion

    }
}
