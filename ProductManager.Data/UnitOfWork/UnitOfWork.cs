using ProductManager.Data.DAO;
using ProductManager.Data.Entities.Common;
using ProductManager.Data.Repositories;
using System.Collections;

namespace ProductManager.Data.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ProductManagerDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(ProductManagerDbContext context)
        {
            _context = context;
        }

        public ICommonRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(CommonRepository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (ICommonRepository<TEntity>)_repositories[type];
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
