using ProductManager.Data.Entities.Common;
using ProductManager.Data.Specifications.Common;
using ProductManager.Shared.Helpers;
using System.Linq.Expressions;

namespace ProductManager.Data.Repositories
{
    public interface ICommonRepository<TEntity> where TEntity : BaseEntity
    {
        Task<bool> Any(Expression<Func<TEntity, bool>> condition);

        Task<List<TEntity>> GetAll();

        Task<TEntity> GetById(int id);

        Task<PagedList<TEntity>> GetAllBySpecification(ICommonSpecification<TEntity> commonSpecification);

        Task<TEntity> GetSingleBySpecification(ICommonSpecification<TEntity> commonSpecification);

        Task Add(TEntity entity);

        Task AddRange(IEnumerable<TEntity> entities);

        Task Delete(int id);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        Task Truncate();
    }
}
