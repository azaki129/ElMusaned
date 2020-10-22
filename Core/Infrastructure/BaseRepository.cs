using Core.Enum;
using Resources.Main;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace TT.Core.Infrastructure
{
    public abstract class BaseRepository<TEntity,Contex> where TEntity : class where Contex: DbContext
    {
        protected Contex context;
        internal DbSet<TEntity> dbSet;

        public BaseRepository(Contex _context)
        {
            context = _context;
            dbSet = context.Set<TEntity>();
        }
        /// <summary>
        /// get list of TEntity based on filter 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        /// <summary>
        /// get TEntity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }
        /// <summary>
        /// Find first occurrence .
        /// Return first or default based on filter
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual TEntity FindFirst(Expression<Func<TEntity, bool>> filter)
        {
            TEntity entity;
           
            if (filter != null)
            {
                entity = dbSet.FirstOrDefault(filter);
            }
            else
            {
                return null;
            }
            return entity;
        }
        /// <summary>
        /// Insert TEntity into database
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }
        /// <summary>
        /// delete TEntity by id
        /// </summary>
        /// <param name="id"></param>
        public ValidMsg Delete(object id)
        {
            TEntity entity = dbSet.Find(id);

            if (entity == null)
            {
                return new ValidMsg
                {
                    IsValid = false,
                    Msg = Main.DeleteFailure + Main.DeletedNotFound,
                    Msg_type = MessageType.error
                };
            }
            try
            {
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                dbSet.Remove(entity);
                context.SaveChanges();

                return new ValidMsg
                {
                    IsValid = true,
                    Msg = Main.DeletedSuccessfully,
                    Msg_type = MessageType.success
                };

            }
            catch (Exception ex)
            {
                return new ValidMsg
                {
                    IsValid = false,
                    Msg = Main.DeleteFoundRelated,
                    Msg_type = MessageType.error
                };
            }
        }

        /// <summary>
        /// Update TEntity 
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            context.SaveChanges();
        }

        public virtual ValidMsg ValidationMessage(TEntity entity, OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Add:
                    return entity == null
                        ? new ValidMsg { IsValid = false, Msg = Main.AddFailure, Msg_type = MessageType.error }
                        : new ValidMsg
                        {
                            IsValid = true,
                            Msg = Main.AddedSuccessfully,
                            Msg_type = MessageType.success
                        };

                case OperationType.Edit:
                    return entity == null
                        ? new ValidMsg
                        {
                            IsValid = false,
                            Msg = Main.EditFailure,
                            Msg_type = MessageType.error
                        }
                        : new ValidMsg
                        {
                            IsValid = true,
                            Msg = Main.EditedSuccessfully,
                            Msg_type = MessageType.success
                        };



                default:
                    return new ValidMsg { IsValid = true, Msg = string.Empty, Msg_type = MessageType.info };
            }
        }
    }
}
