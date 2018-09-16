using System;
using System.Linq;
using System.Linq.Expressions;

namespace WarmUp.Core.DB.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        IQueryable<TEntity> All();
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entity);
    }
}