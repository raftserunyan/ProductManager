using ProductManager.Data.Entities.Common;
using ProductManager.Data.Repositories;

namespace ProductManager.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICommonRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        Task SaveChangesAsync();
    }
}
