using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.Repositories.Interface
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TEntity> Get<TOderKey>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageSize,
                                           Expression<Func<TEntity, TOderKey>> sortKeySelector, bool isAsc = true);

        int Count(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity instance);
        void Update(IEnumerable<TEntity> instances);
        TEntity Add(TEntity instance);
        void Delete(TEntity instance);

    }
}
