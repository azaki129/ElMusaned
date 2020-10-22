using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Delete TEntity by id
        /// </summary>
        /// <param name="id"></param>
        ValidMsg Delete(object id);
        /// <summary>
        /// Get TEntity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        /// <summary>
        /// Get TEntity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(object id);
        /// <summary>
        /// Insert TEntity into database
        /// </summary>
        /// <param name="entity"></param>
        TEntity FindFirst(Expression<Func<TEntity, bool>> filter);
        void Insert(TEntity entity);
        /// <summary>
        /// Update TEntity 
        /// </summary>
        /// <param name="entityToUpdate"></param>
        void Update(TEntity entityToUpdate);
        ValidMsg ValidationMessage(TEntity entity, OperationType operationType);
    }
}
