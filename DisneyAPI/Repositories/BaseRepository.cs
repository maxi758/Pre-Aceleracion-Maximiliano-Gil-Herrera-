
using Microsoft.EntityFrameworkCore;
using DisneyAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DisneyAPI.Repositories
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private DbSet<TEntity> _dbSet;

        protected DbSet<TEntity> DbSet
        {
            get { return _dbSet ??= _dbContext.Set<TEntity>(); }
        }

        protected BaseRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TEntity> GetAllEntities()
        {
            return DbSet.ToList();
        }

        public TEntity Get(int id)
        {
            return DbSet.Find(id);
        }

        public TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _dbContext.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return entity;
        }

        public void Delete(int id)
        {
            var entity = _dbContext.Find<TEntity>(id);
            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
