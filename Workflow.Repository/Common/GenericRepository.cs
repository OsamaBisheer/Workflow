using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Interfaces.ICore;
using Workflow.Domain.Interfaces.IRepositories;

namespace Workflow.Repository.Common
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly IWorkflowDbContext _context;

        public GenericRepository(IWorkflowDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> Find(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return query;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> GetIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(predicate);
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return query;
        }

        public async Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public async Task<TEntity> Add(TEntity insertEntity)
        {
            await _context.Set<TEntity>().AddAsync(insertEntity);
            return insertEntity;
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public TEntity Update(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public void UpdateAll(List<TEntity> entities)
        {
            _context.Set<TEntity>().AttachRange(entities);

            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }
    }
}