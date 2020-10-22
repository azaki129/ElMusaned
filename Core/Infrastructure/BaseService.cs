using Core.Enum;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TT.Core.Infrastructure
{

    /// <summary>
    /// Generic class for service that contains all crud operation and entity manipulation ,
    /// any class inherit from EFService class obtains all those operation without implementing it ,just provied what TDto and TEntity
    /// and all return types are TDto
    /// this class applaying DRY ,Factory,SOLD and other principles
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TEntity"></typeparam>

    public class BaseService<TEntity> where TEntity : class
    {
        /// <summary>
        /// parameter for injection
        /// </summary>
        public IRepository<TEntity> _repository;
        /// <summary>
        /// this constructor to initiate _repository object dynamically 
        /// Accepts any type that implemente IRepository
        /// Applying Open Closed Principle and Factory Pattern
        /// </summary>
        /// <param name="repository"></param>
        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public ValidMsg Delete(object id)
        {
            return _repository.Delete(id);
        }

        public TEntity FindFirst(Expression<Func<TEntity, bool>> filter)
        {
        
            return _repository.FindFirst(filter);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            var entity = _repository.Get((filter), null, includeProperties).ToList();
            return entity;
        }

        public TEntity GetById(object id)
        {
            var entity = _repository.GetById(id);
           
            return entity;
        }

        public void Insert(TEntity entity)
        {
          
            _repository.Insert(entity);
        }

        public void Update(TEntity entity)
        {
     

            _repository.Update(entity);

        }
        public ValidMsg ValidationMessage(TEntity entity, OperationType operationType)
        {
          
            return _repository.ValidationMessage(entity, operationType);
        }
    }
}
