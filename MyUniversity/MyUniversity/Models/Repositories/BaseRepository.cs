using MyUniversity.Models.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.Repositories
{
    /// <summary>
    /// 这是存取数据库数据的基类
    /// </summary>
    /// <typeparam name="TEntity">对应数据库中要查询的表</typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        public UniversityEntities DbContext { get; private set; }
        public DbSet<TEntity> DbSet { get; private set; }
        public BaseRepository(UniversityEntities context)
        {
            this.DbContext = context;
            this.DbSet = this.DbContext.Set<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>表中所有记录</returns>
        public IEnumerable<TEntity> Get()
        {
            return this.DbSet.AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter">使用Lambda表达式的查询条件</param>
        /// <returns></returns>
        ///<example>this.Get(p=>p.ord_id==orderId);查询所有记录时this.Get(true)</example>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return this.DbSet.Where(filter).AsQueryable();
        }

        /// <summary>
        /// 分页查询，并按某个字段排序
        /// </summary>
        /// <typeparam name="TKey">排序字段的类型</typeparam>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页数</param>
        /// <param name="sortKeySelector">排序字段</param>
        /// <param name="isAsc">是否升序排列（不升序则降序排序）</param>
        /// <returns></returns>
        /// <example>this.Get<int>(p=>p.ord_customer==orderCustomer||p.ord_customer=="黄土标",1,10,p=>p.ord_customer,true)</example>
        public IEnumerable<TEntity> Get<TKey>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> sortKeySelector, bool isAsc)
        {
            if (isAsc)
            {
                if (filter == null)
                {
                    return this.DbSet
                        .OrderBy(sortKeySelector)
                        .Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize).AsQueryable();
                }
                else
                {
                    return this.DbSet
                  .Where(filter)
                  .OrderBy(sortKeySelector)
                  .Skip(pageSize * (pageIndex - 1))
                  .Take(pageSize).AsQueryable();
                }

            }
            else
            {
                if (filter == null)
                {
                    return this.DbSet
                        .OrderByDescending(sortKeySelector)
                        .Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize).AsQueryable();
                }
                else
                {
                    return this.DbSet
                    .Where(filter)
                    .OrderByDescending(sortKeySelector)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
                }
            }
        }

        /// <summary>
        /// 获取符合某一查询条件的记录总数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的记录总数</returns>
        ///<example>this.Count(p=>p.pro_quantity>100)</example>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                return this.DbSet.Count();
            }
            else 
            {
                return this.DbSet.Where(predicate).Count();
            }
        }

        public TEntity Add(TEntity instance)
        {
            this.DbSet.Add(instance);
            this.DbContext.SaveChanges();
            return instance;
        }
        public void Update(TEntity instance)
        {
            this.DbSet.Attach(instance);
            this.DbContext.Entry(instance).State = EntityState.Modified;
            this.DbContext.SaveChanges();
        }
        public void Update(IEnumerable<TEntity> instances)
        {
            foreach (TEntity instance in instances)
            {
                this.DbSet.Attach(instance);
                this.DbContext.Entry(instance).State = EntityState.Modified;
            }
            this.DbContext.SaveChanges();
        }
        public void Delete(TEntity instance)
        {
            this.DbContext.Entry(instance).State = EntityState.Deleted;
            this.DbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.DbContext.Dispose();
        }
    }
}
