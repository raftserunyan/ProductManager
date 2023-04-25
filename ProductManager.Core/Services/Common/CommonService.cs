using AutoMapper;
using ProductManager.Core.Models.Common;
using ProductManager.Data.Entities.Common;
using ProductManager.Data.Specifications.Common;
using ProductManager.Data.UnitOfWork;
using ProductManager.Shared.CustomExceptions;
using ProductManager.Shared.Helpers;

namespace ProductManager.Core.Services.Common
{
    internal abstract class CommonService<TModel, TEntity> : ICommonService<TModel, TEntity>
        where TEntity : BaseEntity
        where TModel : BaseModel
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        protected CommonService(IUnitOfWork uow,
                            IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }

        #region Public Methods
        public async Task<int> Add(TModel model)
        {
            if (model == null)
                throw BadRequest($"Model to be added was null");

            var entity = _mapper.Map<TEntity>(model);

            await _unitOfWork.Repository<TEntity>().Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Delete(TModel model)
        {
            if (model == null)
                throw BadRequest("Model to delete cannot be null");

            var entity = _mapper.Map<TEntity>(model);

            _unitOfWork.Repository<TEntity>().Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _unitOfWork.Repository<TEntity>().GetById(id);
            EnsureExists(entity, $"There's no record with id {id} to delete. Entity type: {typeof(TEntity).Name}");

            _unitOfWork.Repository<TEntity>().Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TModel>> GetAll()
        {
            var entities = await _unitOfWork.Repository<TEntity>().GetAll();

            return _mapper.Map<List<TModel>>(entities);
        }

        public async Task<PagedList<TModel>> GetAllPaged(ICommonSpecification<TEntity> specification)
        {
            var entities = await _unitOfWork.Repository<TEntity>().GetAllBySpecification(specification);

            return _mapper.Map<PagedList<TModel>>(entities);
        }

        public async Task<TModel> GetById(int id)
        {
            var entity = await _unitOfWork.Repository<TEntity>().GetById(id);
            EnsureExists(entity, $"Entity with id {id} was not found");

            return _mapper.Map<TModel>(entity);
        }

        public async Task<TModel> GetSingleBySpecification(ICommonSpecification<TEntity> specification)
        {
            var entity = await _unitOfWork.Repository<TEntity>().GetSingleBySpecification(specification);

            return _mapper.Map<TModel>(entity);
        }

        public async Task Update(TModel model)
        {
            if (model == null)
                throw BadRequest($"Model to be updated was null");

            var entity = _mapper.Map<TEntity>(model);

            if (entity.Id == default(int))
                throw BadRequest("Model must have an Id for updating");

            var existingEntity = await _unitOfWork.Repository<TEntity>().GetById(entity.Id);
            EnsureExists(existingEntity, $"There's no record with id {entity.Id} to update");

            _mapper.Map(entity, existingEntity);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Private methods
        protected void EnsureExists<TObject>(TObject entity, string message = "Not found") where TObject : class
        {
            if (entity == null)
            {
                throw new EntityNotFoundException(message);
            }
        }

        protected BadDataException BadRequest(string message = "Bad Request")
        {
            throw new BadDataException(message);
        }
        #endregion
    }
}
