using Microsoft.EntityFrameworkCore;
using ProductManager.Data.DAO;
using ProductManager.Data.Entities.Common;
using ProductManager.Data.Specifications.Common;
using ProductManager.Shared.Helpers;
using System.Linq.Expressions;

namespace ProductManager.Data.Repositories
{
    internal class CommonRepository<TEntity> : ICommonRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ProductManagerDbContext _context;

        public CommonRepository(ProductManagerDbContext context)
        {
            _context = context;
        }

        public async Task Add(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);

            _context.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<List<TEntity>> GetAll()
        {
            var entities = _context.Set<TEntity>().AsQueryable();

            return await entities.ToListAsync();
        }

        public async Task<PagedList<TEntity>> GetAllBySpecification(ICommonSpecification<TEntity> commonSpecification)
        {
            var result = await ApplySpecification(commonSpecification);

            var totalCount = await result.CountAsync();

            if (commonSpecification.IsPagingEnabled)
            {
                result = result.Skip(commonSpecification.Skip)
                             .Take(commonSpecification.Take);
            }

            return new PagedList<TEntity>
            {
                TotalItems = totalCount,
                ItemsPerPage = commonSpecification.Take,
                PageIndex = (commonSpecification.Skip / commonSpecification.Take) + 1,
                Items = await result.ToListAsync()
            };
        }

        public async Task<TEntity> GetSingleBySpecification(ICommonSpecification<TEntity> commonSpecification)
        {
            var result = await ApplySpecification(commonSpecification);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            var entities = _context.Set<TEntity>().AsQueryable();

            return await entities.SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task<IQueryable<TEntity>> ApplySpecification(ICommonSpecification<TEntity> specification)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            query = await SpecificationEvaluator<TEntity>.GetQuery(query, specification);

            return query;
        }

        public async Task Truncate()
        {
            var tableName = _context.Model.FindEntityType(typeof(TEntity)).GetTableName();
            await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {tableName}");
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> condition)
        {
            return await _context.Set<TEntity>().AnyAsync(condition);
        }
    }

    internal class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static async Task<IQueryable<TEntity>> GetQuery(IQueryable<TEntity> inputQuery, ICommonSpecification<TEntity> specification)
        {
            var query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Includes all expression-based includes
            query = specification.Includes.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Include any string-based include statements
            query = specification.IncludeStrings.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Apply ordering if expressions are set
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }            

            return query;
        }
    }
}
