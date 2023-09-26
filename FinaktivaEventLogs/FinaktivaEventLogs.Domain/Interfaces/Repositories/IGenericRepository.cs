using FinaktivaEventLogs.Domain.Common;
using System.Linq.Expressions;

namespace FinaktivaEventLogs.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        ValueTask<TEntity> GetByIdAsync(Guid id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task CreateAsync(TEntity entity);
        Task<Pagination<TEntity>> PaginationByFilter(Pagination<TEntity> pagination, ICollection<string>? includes = null, Boolean? AllResults = false);
        Task<uint> CountByFilter(FilterValue filterValue);
        Task<float> SumByFilter(FilterValue filterValue, string column);
    }
}
