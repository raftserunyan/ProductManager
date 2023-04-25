using ProductManager.Core.Models.Common;
using ProductManager.Data.Entities.Common;
using ProductManager.Data.Specifications.Common;
using ProductManager.Shared.Helpers;

namespace ProductManager.Core.Services.Common
{
    public interface ICommonService<TModel, TEntity>
        where TEntity : BaseEntity
        where TModel : BaseModel
    {
        Task<TModel> GetById(int id);
        Task<List<TModel>> GetAll();
        Task<PagedList<TModel>> GetAllPaged(ICommonSpecification<TEntity> specification);
        Task<TModel> GetSingleBySpecification(ICommonSpecification<TEntity> specification);
        Task<int> Add(TModel model);
        Task Update(TModel model);
        Task Delete(TModel model);
        Task Delete(int id);
    }
}
