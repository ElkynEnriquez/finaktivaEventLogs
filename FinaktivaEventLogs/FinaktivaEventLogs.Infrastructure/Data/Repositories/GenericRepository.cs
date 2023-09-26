using FinaktivaEventLogs.Domain.Common;
using FinaktivaEventLogs.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace FinaktivaEventLogs.Infrastructure.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _appDbContext;
        public GenericRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #region Métodos Sincrónicos

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _appDbContext.Set<TEntity>().Where(predicate);
        }

        public void Create(TEntity entity)
        {
            _appDbContext.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            _appDbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            _appDbContext.Set<TEntity>().Remove(entity);
        }

        #endregion

        #region Métodos Asincrónicos
        public ValueTask<TEntity> GetByIdAsync(Guid id)
        {
            return _appDbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _appDbContext.Set<TEntity>().AddAsync(entity);
        }
        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _appDbContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }
        #endregion
        #region Métodos Paginación, conteo y sumatorias
        // para conteos
        public async Task<uint> CountByFilter(FilterValue filterValue)
        {
            IQueryable<TEntity> query = _appDbContext.Set<TEntity>();
            // Si hay un filtro, aplicar la condición correspondiente
            if (filterValue != null && !String.IsNullOrEmpty(filterValue.Key))
            {
                // Obtener los valores del filtro como un arreglo
                var filterValues = filterValue.FilterValues;
                // Aplicar el filtro a la consulta
                query = query.Where(filterValue.Key, filterValues);
            }
            // Variable para sumar elementos coincidentes
            // almacena suma de elementos encontró
            var totalItems = (float)query.Count();
            return (uint)totalItems;
        }
        // sumatorias
        public async Task<float> SumByFilter(FilterValue filterValue, string column)
        {
            IQueryable<TEntity> query = _appDbContext.Set<TEntity>();
            // Si hay un filtro, aplicar la condición correspondiente
            if (filterValue != null && !String.IsNullOrEmpty(filterValue.Key))
            {
                // Obtener los valores del filtro como un arreglo
                var filterValues = filterValue.FilterValues;
                // Aplicar el filtro a la consulta
                query = query.Where(filterValue.Key, filterValues);
            }
            // Variable para sumar elementos coincidentes
            // almacena suma de elementos encontró
            var total = (float)query.Sum(column);
            return total;
        }
        // Paginación

        public async Task<Pagination<TEntity>> PaginationByFilter(Pagination<TEntity> pagination, ICollection<string>? includes, Boolean? allResults)
        {
            IQueryable<TEntity> query = _appDbContext.Set<TEntity>();
            // Si hay un filtro, aplicar la condición correspondiente
            if (pagination.FilterValue != null && !String.IsNullOrEmpty(pagination.FilterValue.Key))
            {
                // Obtener los valores del filtro como un arreglo
                var filterValues = pagination.FilterValue.FilterValues;
                // Aplicar el filtro a la consulta
                query = query.Where(pagination.FilterValue.Key, filterValues);
            }

            // Obtener el total de elementos sin paginación
            var totalItems = await query.CountAsync();

            // Ordenar y paginar la consulta
            query = pagination.SortDirection == "desc"
                ? query.OrderBy(pagination.Sort + " desc")
                : query.OrderBy(pagination.Sort);
            if (allResults == false)
            {
                query = query.Skip((pagination.Page) * pagination.PageSize)
                             .Take(pagination.PageSize);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            pagination.Data = await query.ToListAsync();

            // Calcular la cantidad total de páginas
            var totalPages = (int)Math.Ceiling(totalItems / (decimal)pagination.PageSize);
            pagination.PagesQuantity = totalPages;
            pagination.TotalRows = (int)totalItems;

            return pagination;
        }
        #endregion
    }
}
