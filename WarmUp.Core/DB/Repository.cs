using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using WarmUp.Core.DB.Interfaces;

namespace WarmUp.Core.DB
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private RSSContext _context;

        public Repository(RSSContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            _context.SaveChanges();
        }

        public IQueryable<TEntity> All()
        {
            return _context.Set<TEntity>();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}