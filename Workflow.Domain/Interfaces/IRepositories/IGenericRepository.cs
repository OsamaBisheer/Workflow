using System.Linq.Expressions;

namespace Workflow.Domain.Interfaces.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Find(int id);

        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Add(TEntity entity);

        Task AddRange(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);

        void UpdateAll(List<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task<int> Commit();
    }
}